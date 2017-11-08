using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortafolioWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection data)
        {
            string codigo = data["codigo"].ToString();
            bool resultado = ValidarSolicitud(codigo);
            if(resultado)
                TempData["SuccessInsert"] = "El permiso es válido";
            else
                TempData["SuccessInsert"] = "El permiso no existe";
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
    }
}