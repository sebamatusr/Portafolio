using Newtonsoft.Json;
using PortafolioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortafolioWeb.Controllers
{
    public class JefeSuperiorController : Controller
    {
        Entities db = new Entities();
        SolicitudViewModel model = new SolicitudViewModel();


        // GET: JefeSuperior
        public ActionResult Index()
        {
            if (Session["rol_name"].Equals("Jefe Unidad Superior"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ResolucionForm()
        {
            
            if (Session["rol_name"].Equals("Jefe Unidad Superior"))
            {
                model.Unidades = db.UNIDAD.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult ResolucionForm(FormCollection col)
        {
            int idunidad = 0;
            model.Unidades = db.UNIDAD.ToList();
            try
            {
                idunidad = Convert.ToInt32(col["Unidades"]);
            }
            catch (Exception)
            {
                TempData["error"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>Ha ocurrido un error, verifique que ha seleccionado una unidad.</strong></div>"; 
                return View(model);
            }
           

            UNIDAD unidadent = model.Unidades.Where(u => u.ID_UNIDAD == idunidad).FirstOrDefault();

            string unidad = unidadent.NOMBRE_UNIDAD;

            //r => r.UNIDAD.ToLower().Equals(unidad.ToLower()) && 

            int currentMonth = DateTime.Now.AddMonths(-1).Month;

            var resolucion = db.HISTORIAL_RESOLUCIONES.Where(r => r.FECHA_EMISION.Year == DateTime.Now.Year 
                                                             && r.FECHA_EMISION.Month == currentMonth
                                                             && r.UNIDAD.Equals(unidad)).FirstOrDefault();

            if(resolucion!=null)
            {
                TempData["GenerarResult"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>Esta resolución ya fue generada.</strong></div>";
                return View(model);
            }
            else
            {

                model.Solicitudes = db.SOLICITUD.Where(s => s.FECHA_INICIO.Year == DateTime.Now.Year && s.FECHA_INICIO.Month == DateTime.Now.Month).ToList();
                
                if(model.Solicitudes != null)
                {
                    List<SOLICITUD> listasol = new List<SOLICITUD>();
                    foreach (var item in model.Solicitudes)
                    {
                        FUNCIONARIO tempfunc = db.FUNCIONARIO.Where(f => f.RUT.Equals(item.RUT_FUNCIONARIO)).FirstOrDefault();
                        if (tempfunc.ID_UNIDAD == idunidad)
                        {
                            listasol.Add(item);
                        }
                    }
                    if(listasol.Count < 1)
                    {
                        TempData["GenerarResult"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>No existen solicitudes en el período y unidad seleccionados.</strong></div>";
                        return View(model);
                    }
                    else
                    {
                        HISTORIAL_RESOLUCIONES nuevaresolucion = new HISTORIAL_RESOLUCIONES();
                        //CAMBIAR RUT POR SESSION RUT
                        nuevaresolucion.RUT_EMISOR = "12796158-1";
                        nuevaresolucion.FECHA_EMISION = DateTime.Now.AddMonths(-1);
                        nuevaresolucion.UNIDAD = unidad;

                        db.Configuration.AutoDetectChangesEnabled = true;
                        db.HISTORIAL_RESOLUCIONES.Add(nuevaresolucion);
                        db.ChangeTracker.HasChanges();
                        db.SaveChanges();
                        TempData["GenerarResult"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>Resolución fue generada con éxito.</strong></div>";
                        return View(model);
                    }
                }
                else
                {
                    TempData["GenerarResult"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>No existen solicitudes en el período y unidad seleccionados.</strong></div>";
                    return View(model);
                }

                
            }
            
        }

        public ActionResult ConsultarResolucion()
        {
            if (Session["rol_name"].Equals("Jefe Unidad Superior"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult GetResoluciones()
        {
            db.Configuration.LazyLoadingEnabled = false;
            SolicitudViewModel model = new SolicitudViewModel();
            var resoluciones = db.HISTORIAL_RESOLUCIONES.ToList();

            var reslinq = (from h in db.HISTORIAL_RESOLUCIONES join u in db.UNIDAD on h.UNIDAD equals u.NOMBRE_UNIDAD select new
            {
                h.FECHA_EMISION, h.RUT_EMISOR, h.UNIDAD, h.ID_RESOLUCION, u.ID_UNIDAD
            }).ToList();

            object jsonO = new { data = reslinq };
            var json = JsonConvert.SerializeObject(reslinq, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
            //JObject jsonObj = JObject.Parse(json);

            return Content(json, "application/json");
        }
        public ActionResult Resolucion(int id)
        {
            
            if (Session["rol_name"].Equals("Jefe Unidad Superior"))
            {
                TempData["id_unidad"] = id.ToString();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListarFuncionariosPorUnidad()
        {
            
            if (Session["rol_name"].Equals("Jefe Unidad Superior"))
            {
                model.Unidades = db.UNIDAD.ToList();
                TempData["unidad"] = 0;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult ListarFuncionariosPorUnidad(FormCollection col)
        {
            model.Unidades = db.UNIDAD.ToList();
            try
            {
                TempData["unidad"] = Convert.ToInt32(col["Unidades"]);
                return View(model);
            }
            catch (Exception)
            {
                TempData["unidad"] = 0;
                return View(model);
            }
            
            
        }
        public ActionResult GetFuncionariosPorUnidad(int unidad)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var funcionarios = (from f in db.FUNCIONARIO
                                where f.ESTADO == 1 && f.ID_UNIDAD == unidad
                                select new
                                {
                                    f.RUT,
                                    NOMBRECOMPLETO = f.NOMBRE + " " + f.APELLIDO_PATERNO,
                                    f.EMAIL
                                }).ToList();
            var json = JsonConvert.SerializeObject(funcionarios, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
            return Content(json, "application/json");
        }
        public ActionResult ResumenReport(int id)
        {
            
            if (Session["rol_name"].Equals("Jefe Unidad Superior"))
            {
                TempData["id_unidad"] = id.ToString();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ResumenCuantitativo()
        {
            

            if (Session["rol_name"].Equals("Jefe Unidad Superior"))
            {
                model.Unidades = db.UNIDAD.ToList();

                model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult GetUnidades()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var reslinq = (from u in db.UNIDAD
                           join f in db.FUNCIONARIO on u.ID_UNIDAD equals f.ID_UNIDAD
                           join r in db.ROL on f.ID_ROL equals r.ID_ROL
                           where r.NOMBRE_ROL.Equals("Jefe Unidad Interior")
                           select new
                           {
                               u.ID_UNIDAD,
                               u.NOMBRE_UNIDAD,
                               NOMBRE_JEFE = f.NOMBRE + " " + f.APELLIDO_PATERNO
                           }).ToList();


            var json = JsonConvert.SerializeObject(reslinq, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });

            return Content(json, "application/json");
        }
    }
}