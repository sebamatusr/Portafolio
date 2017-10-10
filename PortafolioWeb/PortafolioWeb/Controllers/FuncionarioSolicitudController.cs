using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortafolioWeb;
using System.Data;

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
            if (!Session["rol_name"].Equals("Funcionario"))
            {
                return RedirectToAction("Index", "LoginController");
            }
            return View();
        }
        public ActionResult ConsultarPermisos()
        {
            if (!Session["rol_name"].Equals("Funcionario"))
            {
                return RedirectToAction("Index", "LoginController");
            }
            return View();
        }
        // GET: FuncionarioSolicitud/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FuncionarioSolicitud/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FuncionarioSolicitud/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
