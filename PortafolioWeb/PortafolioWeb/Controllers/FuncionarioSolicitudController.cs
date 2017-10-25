using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortafolioWeb;
using System.Data;
using PortafolioWeb.Models;
using PortafolioWeb.WebServiceRH;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                return RedirectToAction("Index", "LoginController");
            }
            return View();
        }
        public ActionResult FuncionarioSolicitud()
        {
            try
            {
                if (!Session["rol_name"].Equals("Funcionario"))
                {
                    return RedirectToAction("Index", "LoginController");
                }

                SolicitudViewModel model = new SolicitudViewModel();

                model.Solicitudes = db.SOLICITUD.ToList();
                model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();
                model.Motivos = db.MOTIVO.ToList();


                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "LoginController");
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

        public ActionResult GetPermisos(int id_tiposolicitud, string rut)
        {
            if (id_tiposolicitud <= 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            db.Configuration.LazyLoadingEnabled = false;

            var solicitudes = db.SOLICITUD.Where(c => c.ID_ESTADO == 2 &&
                                                 c.RUT_FUNCIONARIO.Equals(rut) &&
                                                 c.ID_TIPOSOLICITUD == id_tiposolicitud &&
                                                 c.FECHA_INICIO.Year == DateTime.Now.Year).ToList();

            int DiasSolicitud = Convert.ToInt32((from d in db.TIPO_SOLICITUD where d.ID_TIPOSOLICITUD == id_tiposolicitud select d.CANTIDAD_DIAS).FirstOrDefault());

            string tipo_permiso = (from p in db.TIPO_SOLICITUD where p.ID_TIPOSOLICITUD == id_tiposolicitud select p.DESCRIPCION).FirstOrDefault().ToString();

            if (solicitudes != null)
            {
                switch (tipo_permiso)
                {
                    case ("Permiso Administrativo"):
                        int diasUsados = 0;
                        foreach (var item in solicitudes)
                        {
                            DateTime fechaCorte = GetFechaCorte(rut, item.FECHA_INICIO, item.FECHA_FIN);
                            int dias = (item.FECHA_FIN - item.FECHA_INICIO).Days;
                            diasUsados += dias;
                        }
                        int diasDisponibles = DiasSolicitud - diasUsados;
                        return Json(diasDisponibles, JsonRequestBehavior.AllowGet);
                    default:
                        diasUsados = 0;
                        foreach (var item in solicitudes)
                        {
                            int dias = (item.FECHA_FIN - item.FECHA_INICIO).Days;
                            diasUsados += dias;
                        }
                        diasDisponibles = DiasSolicitud - diasUsados;
                        return Json(diasDisponibles, JsonRequestBehavior.AllowGet);
                }
            }



            return Json(solicitudes, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ConsultarPermisos()
        {
            if (Session["rol_name"] == null || !Session["rol_name"].Equals("Funcionario"))
            {
                TempData["mensajeError"] = "Debe estar logueado para realizar esta acción";
                return RedirectToAction("Index", "LoginController");
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
                solicitud.ID_ESTADO = 1;
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

                //int dias = BusinessDaysUntil(fechaInicio, fechaFin, feriados.feriados);
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
            return View();
        }

        // POST: FuncionarioSolicitud/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FuncionarioSolicitud/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FuncionarioSolicitud/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
