using Newtonsoft.Json;
using PortafolioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortafolioWeb.Controllers
{
    public class AdminController : Controller
    {
        Entities db = new Entities();
        SolicitudViewModel model = new SolicitudViewModel();
        public ActionResult ValidarPermiso()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ValidarPermiso(FormCollection data)
        {
            string codigo = data["codigo"].ToString();
            bool resultado = ValidarSolicitud(codigo);
            if (resultado)
                TempData["SuccessInsert"] = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>El permiso es válido.</strong></div>";
            else
                TempData["SuccessInsert"] = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>×</a><strong>El permiso no existe.</strong></div>";
            return View();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        private static bool ValidarSolicitud(string codigo)
        {
            bool valido = false;

            SOLICITUD sol = new Entities().SOLICITUD.Where(s => s.CODIGO_VERIFICACION.Equals(codigo)).FirstOrDefault();

            if (sol != null)
            {
                valido = true;
            }

            return valido;
        }
        public ActionResult SalidaXML()
        {
            model.Unidades = db.UNIDAD.ToList();
            TempData["unidad"] = 0;
            return View(model);
        }
        [HttpPost]
        public ActionResult SalidaXML(FormCollection col)
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
            var funcionarios = (from s in db.SOLICITUD
                                join f in db.FUNCIONARIO on s.RUT_FUNCIONARIO equals f.RUT into loj
                                from f in loj.DefaultIfEmpty()
                                where f.ESTADO == 1 && f.ID_UNIDAD == unidad
                                select new
                                {
                                    f.RUT,
                                    NOMBRECOMPLETO = f.NOMBRE + " " + f.APELLIDO_PATERNO,
                                    f.EMAIL
                                }).Distinct().ToList();
            var json = JsonConvert.SerializeObject(funcionarios, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
            return Content(json, "application/json");
        }
    }
}