using Newtonsoft.Json;
using PortafolioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortafolioWeb.Controllers
{
    public class AlcaldeController : Controller
    {
        Entities db = new Entities();
        SolicitudViewModel model = new SolicitudViewModel();
        // GET: Alcalde
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConsultarFuncionarios()
        {
            model.Unidades = db.UNIDAD.ToList();
            TempData["unidad"] = 0;
            return View(model);
        }
        [HttpPost]
        public ActionResult ConsultarFuncionarios(FormCollection col)
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
        public ActionResult ConsultarResoluciones()
        {
            return View();
        }
        public ActionResult VerificarPermiso()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerificarPermiso(FormCollection data)
        {
            string codigo = data["codigo"].ToString();
            bool resultado = ValidarSolicitud(codigo);
            if (resultado)
                TempData["SuccessInsert"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>El permiso es válido.</strong></div>";
            else
                TempData["SuccessInsert"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>El permiso no existe.</strong></div>";
            return View();
        }
        private bool ValidarSolicitud(string codigo)
        {
            bool valido = false;

            SOLICITUD sol = db.SOLICITUD.Where(s => s.CODIGO_VERIFICACION.Equals(codigo)).FirstOrDefault();

            if (sol != null)
            {
                valido = true;
            }

            return valido;
        }
        public ActionResult ResumenCuantitativo()
        {
            model.Unidades = db.UNIDAD.ToList();
            model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult ResumenCuantitativo(FormCollection col)
        {
            model.Unidades = db.UNIDAD.ToList();
            TempData["unidad"] = Convert.ToInt32(col["Unidades"]);

            return View(model);
        }
        public ActionResult ResumenReport(int id)
        {
            TempData["id_unidad"] = id.ToString();
            return View();
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
        public ActionResult GetResoluciones()
        {
            db.Configuration.LazyLoadingEnabled = false;
            SolicitudViewModel model = new SolicitudViewModel();
            var resoluciones = db.HISTORIAL_RESOLUCIONES.ToList();

            var reslinq = (from h in db.HISTORIAL_RESOLUCIONES
                           join u in db.UNIDAD on h.UNIDAD equals u.NOMBRE_UNIDAD
                           select new
                           {
                               h.FECHA_EMISION,
                               h.RUT_EMISOR,
                               h.UNIDAD,
                               h.ID_RESOLUCION,
                               u.ID_UNIDAD
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
    }
}