using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortafolioWeb;

namespace PortafolioWeb.Controllers
{
    public class LoginController : Controller
    {
        private Entities db = new Entities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "RUT,PASSWORD")]  FUNCIONARIO user)
        {

            string username = user.RUT;
            string password = user.PASSWORD;
            try
            {

                int credencialesVadlidas = db.LoginValidador(username, password);

                if (credencialesVadlidas == 1)
                {
                    FUNCIONARIO usuario = (from src in db.FUNCIONARIO
                                           where src.RUT.Equals(username)
                                           select src).FirstOrDefault();
                    if (usuario != null)
                    {
                        Session["rol_name"] = usuario.ROL.NOMBRE_ROL;
                        Session.Add("rol_id", (int)(usuario.ID_ROL));
                        Session.Add("id_unidad", (int)(usuario.ID_UNIDAD));
                        Session.Add("rut", usuario.RUT);
                        Session.Add("nombre", usuario.NOMBRE);
                        string unidad = (from src in db.UNIDAD
                                         where src.ID_UNIDAD == usuario.ROL.UNIDAD.ID_UNIDAD
                                         select src.NOMBRE_UNIDAD).FirstOrDefault();
                        Session.Add("unidad", unidad);
                        if (Session["rol_name"].Equals("Administrador"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        if (Session["rol_name"].Equals("Jefe Unidad Superior"))
                        {
                            return RedirectToAction("Index", "JefeSuperior");
                        }
                        else if (Session["rol_name"].Equals("Funcionario"))
                        {
                            return RedirectToAction("Index", "FuncionarioSolicitud");
                        }
                        else if (Session["rol_name"].Equals("Alcalde"))
                        {
                            return View("JefeUnidadSuperior", db.FUNCIONARIO);
                        }
                        else if (Session["rol_name"].Equals("Jefe Unidad Interior"))
                        {
                            return RedirectToAction("Index", "JefeInterno");
                        }

                    }
                }


                //añadir @Html.Raw(TempData["msg"]) en la vista
                //TempData["msg"] = "<script>alert('Credenciales inválidas');</script>";
                TempData["LoginInvalido"] = "LoginInvalido";
                return View();
            }
            catch (Exception ex)
            {
                TempData["Excepcion"] = ex.Message;
                return View();
            }


        }
        public ActionResult Administrador()
        {
            return RedirectToAction("Index", "AdminController");
        }
        public ActionResult JefeUnidadInterior()
        {
            return View();
        }
        public ActionResult Alcalde()
        {
            if(Session["rol_name"].Equals("Alcalde"))
            {
                return View();
            }

            else
                return Redirect("Index");
        }
        public ActionResult Funcionario()
        {
            if (Session["rol_name"].Equals("Funcionario"))
            {
                return RedirectToAction("Index","FuncionarioController");
            }

            else
                return Redirect("Index");
        }
        public ActionResult SolicitarPermiso()
        {
            if (Session["rol_name"] != null)
            {
                if (Session["rol_name"].Equals("Funcionario"))
                {
                    return View("SolicitarPermiso");
                }
                else
                {
                    return View("Index");
                }
            }
            return Redirect("Index");
        }
        public ActionResult PermisosSelf()
        {
            if (Session["rol_name"] != null)
            {
                if (Session["rol_name"].Equals("Funcionario"))
                {
                    return View("PermisosSelf");
                }
                else
                {
                    return View("Index");
                }
            }
            return Redirect("Index");
        }
    

        //GET
        public ActionResult JefeUnidadSuperior()
        {
            if (Session["rol_name"].Equals("Jefe Unidad Superior") || Session["rol_name"].Equals("Alcalde"))
            {
                return View();
            }
                
            else
                return View("Index");
        }
        public ActionResult GenerarResoluciones()
        {
            if (Session["rol_name"] != null)
            {
                if (Session["rol_name"].Equals("Jefe Unidad Superior") || Session["rol_name"].Equals("Alcalde"))
                {
                    var model = db.UNIDAD;
                    return View("GenerarResoluciones", model.ToList());
                }
                else
                {
                    return View("Index");
                }
            }
            return View("Index");
                
        }
        public ActionResult ConsultarPermisos()
        {
            if (Session["rol_name"] != null)
            {
                if (Session["rol_name"].Equals("Jefe Unidad Superior") || Session["rol_name"].Equals("Alcalde"))
                    {
                    return View("ConsultarPermisos");
                }
                else
                {
                    return View("Index");
                }
            }
            return View("Index");
        }


        public ActionResult EndSession()
        {
            Session.Abandon();
            return Redirect("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
