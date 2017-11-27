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
            return View();
        }
        public ActionResult ResolucionForm()
        {
            model.Unidades = db.UNIDAD.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult ResolucionForm(FormCollection col)
        {
            model.Unidades = db.UNIDAD.ToList();
            int idunidad = Convert.ToInt32(col["Unidades"]);

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
                        TempData["GenerarResult"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>No existen solicitudes en el período y unidad seleccionados.</strong></div>";
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
                    TempData["GenerarResult"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>No existen solicitudes en el período y unidad seleccionados.</strong></div>";
                    return View(model);
                }

                
            }
            
        }

        public ActionResult ConsultarResolucion()
        {
            return View();
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

            return View();
        }
    }
}