using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using PortafolioWeb.Models;
using PortafolioWeb.WebServiceRH;
using Newtonsoft.Json;

namespace PortafolioWeb.Controllers
{
    public class FuncionarioSolicitudController : Controller
    {
        private Entities db = new Entities();

        // GET: FuncionarioSolicitud
        public ActionResult Index()
        {
            if (!Session["rol_name"].Equals("Funcionario"))
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult FuncionarioSolicitud()
        {
            try
            {
                if (!Session["rol_name"].Equals("Funcionario"))
                {
                    return RedirectToAction("Index", "Login");
                }

                SolicitudViewModel model = new SolicitudViewModel();

                model.Solicitudes = db.SOLICITUD.ToList();
                model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();
                model.Motivos = db.MOTIVO.ToList();


                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Login");
            }

        }
        public ActionResult GetMotivos(int id_tiposolicitud)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var motivos = db.MOTIVO.Where(c => c.ID_TIPOSOLICITUD == id_tiposolicitud).ToList();

            return Json(motivos, JsonRequestBehavior.AllowGet);
        }
        public DateTime GetFechaCorte(string rut, DateTime inicio, DateTime fin)
        {
            AsistenciaClient webservice = new AsistenciaClient();
            var dates = new List<DateTime>();

            for (var dt = inicio; dt <= fin; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            foreach (var item in dates)
            {
                if (webservice.GetAsistencia(rut, item))
                {
                    return item;
                }
            }
            return fin.AddDays(1);
            //return (fin.AddDays(1) - inicio).Days;
        }

        public JsonResult GetPermisos(int id_tiposolicitud, string rut)
        {
            if (id_tiposolicitud <= 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            db.Configuration.LazyLoadingEnabled = false;

            var solicitudes = db.SOLICITUD.Where(c => c.ID_ESTADO == 11 &&
                                                 c.RUT_FUNCIONARIO.Equals(rut) &&
                                                 c.ID_TIPOSOLICITUD == id_tiposolicitud &&
                                                 c.FECHA_INICIO.Year == DateTime.Now.Year).ToList();

            int DiasSolicitud = Convert.ToInt32((from d in db.TIPO_SOLICITUD where d.ID_TIPOSOLICITUD == id_tiposolicitud select d.CANTIDAD_DIAS).FirstOrDefault());

            string tipo_permiso = (from p in db.TIPO_SOLICITUD where p.ID_TIPOSOLICITUD == id_tiposolicitud select p.DESCRIPCION).FirstOrDefault().ToString();

            if (solicitudes != null)
            {
                int diasUsados = 0;
                int diasDisponibles = 0;
                switch (tipo_permiso)
                {
                    case ("Feriado Legal Anual"):
                        AsistenciaClient webservice = new AsistenciaClient();
                        int antique = webservice.GetAntiguedad(rut)/12;
                        foreach (var item in solicitudes)
                        {
                            DateTime fechaCorte = GetFechaCorte(rut, item.FECHA_INICIO, item.FECHA_FIN);
                            int dias = (item.FECHA_FIN - item.FECHA_INICIO).Days;
                            diasUsados += dias;
                        }
                        if (antique>=1 && antique < 15)
                        {
                            DiasSolicitud = 15;
                        }
                        else if (antique >= 15 && antique < 20)
                        {
                            DiasSolicitud = 20;
                        }
                        diasDisponibles = DiasSolicitud - diasUsados;
                        return Json(diasDisponibles, JsonRequestBehavior.AllowGet);
                    default:
                        diasUsados = 0;
                        foreach (var item in solicitudes)
                        {
                            DateTime fechaCorte = GetFechaCorte(rut, item.FECHA_INICIO, item.FECHA_FIN);
                            int dias = (item.FECHA_FIN - item.FECHA_INICIO).Days;
                            diasUsados += dias;
                        }
                        diasDisponibles = DiasSolicitud - diasUsados;
                        return Json(diasDisponibles, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(DiasSolicitud, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ConsultarPermisos()
        {
            if (Session["rol_name"] == null || !Session["rol_name"].Equals("Funcionario"))
            {
                TempData["mensajeError"] = "Debe estar logueado para realizar esta acción";
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult GetPermisosPorRut(string rut)
        {
            db.Configuration.LazyLoadingEnabled = false;
            SolicitudViewModel model = new SolicitudViewModel();
            var solicitudes = db.SOLICITUD.Where(c => c.RUT_FUNCIONARIO.Equals(rut)).ToList();
            object jsonO = new { data = solicitudes };
            var json = JsonConvert.SerializeObject(solicitudes, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
            //JObject jsonObj = JObject.Parse(json);

            return Content(json, "application/json");
        }
        // GET: FuncionarioSolicitud/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // POST: FuncionarioSolicitud/Create
        [HttpPost]
        public ActionResult FuncionarioSolicitud(FormCollection collection)
        {
            try
            {
                SolicitudViewModel model = new SolicitudViewModel();

                // TODO: Add insert logic here
                string guid = Guid.NewGuid().ToString();

                SOLICITUD solicitud = new SOLICITUD();
                solicitud.CODIGO_VERIFICACION = guid;
                solicitud.ID_ESTADO = 10;
                var idsol = collection["Tipos_Solicitud"];
                solicitud.ID_TIPOSOLICITUD = Convert.ToInt32(idsol);
                solicitud.RUT_FUNCIONARIO = Session["rut"].ToString();
                solicitud.DESCRIPCION = collection["comment"];
                solicitud.FECHA_CREACION = DateTime.Now;
                solicitud.FECHA_MODIFICACION = DateTime.Now;

                string fechaInputInicio = collection["solicitud.FECHA_INICIO"];
                string fechaInputFin = collection["solicitud.FECHA_FIN"];

                IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                DateTime fechaInicio = DateTime.Parse(fechaInputInicio, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                DateTime fechaFin = DateTime.Parse(fechaInputFin, culture, System.Globalization.DateTimeStyles.AssumeLocal);

                Root feriados = FeriadosJSONcall.GetFeriados();

                int dias = DateTimeExtention.BusinessDaysUntil(fechaInicio, fechaFin, feriados.feriados);
                solicitud.FECHA_INICIO = fechaInicio;
                solicitud.FECHA_FIN = fechaFin;
                solicitud.ESTADO = 1;
                solicitud.ID_SOLICITUD = 1;



                model.Solicitudes = db.SOLICITUD.ToList();
                model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();
                model.Motivos = db.MOTIVO.ToList();

                db.Configuration.AutoDetectChangesEnabled = true;
                db.SOLICITUD.Add(solicitud);
                db.ChangeTracker.HasChanges();
                db.SaveChanges();

                TempData["SuccessInsert"] = "Solicitud agregada correctamente";
                return View(model);

            }
            catch(Exception ex)
            {
                string msgEx = ex.Message;
                return RedirectToAction("Index");
            }
        }

        

        // GET: FuncionarioSolicitud/Edit/5
        public ActionResult Edit(int id)
        {
            using (db)
            {
                SolicitudViewModel model = new SolicitudViewModel();



                model.Solicitudes = db.SOLICITUD.ToList();
                model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();
                model.Motivos = db.MOTIVO.ToList();
                model.solicitud = model.SOLICITUD.Where(a => a.ID_SOLICITUD == id).FirstOrDefault();
                return View(model);
            }
        }

        // POST: FuncionarioSolicitud/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            
            try
            {
                bool status = false;
                if(Convert.ToInt32(collection["solicitud.ID_SOLICITUD"]) > 0)
                {
                    int idsol = Convert.ToInt32(collection["solicitud.ID_SOLICITUD"]);
                    var permiso = db.SOLICITUD.Where(a => a.ID_SOLICITUD == idsol).FirstOrDefault();
                    if (permiso != null)
                    {
                        permiso.FECHA_MODIFICACION = DateTime.Now;

                        string fechaInputInicio = collection["solicitud.FECHA_INICIO"];
                        string fechaInputFin = collection["solicitud.FECHA_FIN"];

                        IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                        DateTime fechaInicio = DateTime.Parse(fechaInputInicio, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                        DateTime fechaFin = DateTime.Parse(fechaInputFin, culture, System.Globalization.DateTimeStyles.AssumeLocal);

                        Root feriados = FeriadosJSONcall.GetFeriados();

                        permiso.FECHA_INICIO = fechaInicio;
                        permiso.FECHA_FIN = fechaFin;
                        permiso.DESCRIPCION = collection["comment"];
                        var idtipo = collection["Tipos_Solicitud"];
                        permiso.ID_TIPOSOLICITUD = Convert.ToInt32(idtipo);
                        db.Configuration.AutoDetectChangesEnabled = true;
                        db.ChangeTracker.HasChanges();
                        db.SaveChanges();
                        status = true;
                    }
                }
                return new JsonResult { Data = new { status = status } };


            }
            catch (Exception ex)
            {
                string msgEx = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: FuncionarioSolicitud/Delete/5
        public ActionResult Delete(int id)
        {
            using (db)
            {
                var permiso = db.SOLICITUD.Where(a => a.ID_SOLICITUD == id).FirstOrDefault();
                if (permiso != null)
                {
                    return View(permiso);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        // POST: FuncionarioSolicitud/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                bool status = false;
                using (db)
                {
                    var permiso = db.SOLICITUD.Where(a => a.ID_SOLICITUD == id).FirstOrDefault();

                    if (permiso != null)
                    {
                        if (permiso.ESTADO1.DESCRIPCION.Equals("Pendiente"))
                        {
                            permiso.ESTADO = 0;
                            permiso.ESTADO1 = db.ESTADO.Where(e => e.ID_ESTADO == permiso.ID_ESTADO).FirstOrDefault();
                            db.Configuration.AutoDetectChangesEnabled = true;
                            db.ChangeTracker.HasChanges();
                            db.SaveChanges();
                            status = true;
                        }
                        else
                        {
                            return new JsonResult { Data = new { status = status } };
                        }
                    }
                }

                return new JsonResult { Data = new { status = status } };
            }
            catch (Exception ex)
            {
                string msgEx = ex.Message;
                return View();
            }
        }
    }
}
