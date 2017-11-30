using Newtonsoft.Json;
using PortafolioWeb.CustomXML;
using PortafolioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace PortafolioWeb.Controllers
{
    public class DesempenoController : ApiController
    {
        Entities db = new Entities();
        // GET: api/ResolucionMensual
        public HttpResponseMessage Get()
        {
            db.Configuration.LazyLoadingEnabled = false;


            var reslinq = (from f in db.FUNCIONARIO
                           join s in db.SOLICITUD on f.RUT equals s.RUT_FUNCIONARIO
                           join t in db.TIPO_SOLICITUD on s.ID_TIPOSOLICITUD equals t.ID_TIPOSOLICITUD
                           join u in db.UNIDAD on f.ID_UNIDAD equals u.ID_UNIDAD
                           select new
                           {
                               f.NOMBRE,
                               f.RUT,
                               u.NOMBRE_UNIDAD,
                               s.FECHA_INICIO,
                               s.FECHA_FIN,
                               t.DESCRIPCION
                           }).ToList();

            var permisos = new XElement("root");
            
            foreach (var item in reslinq)
            {
                permisos.Add(
                    new XElement("permiso", new XElement("funcionario",
                new XElement("nombre", item.NOMBRE),
                new XElement("rut", item.RUT),
                new XElement("unidad", item.NOMBRE_UNIDAD)),
            new XElement("detalle", new XElement("fecha_inicio", item.FECHA_INICIO.ToString("dd-MM-yyyy")),
                new XElement("fecha_fin", item.FECHA_FIN.ToString("dd-MM-yyyy")),
                new XElement("descripcion", item.DESCRIPCION)))
                    );
            }
            return new HttpResponseMessage() { Content = new StringContent(string.Concat(permisos), Encoding.UTF8, "application/xml") };
        }

        // GET: api/ResolucionMensual/5
        public HttpResponseMessage Get(string rut)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var reslinq = (from f in db.FUNCIONARIO
                           join s in db.SOLICITUD on f.RUT equals s.RUT_FUNCIONARIO
                           join t in db.TIPO_SOLICITUD on s.ID_TIPOSOLICITUD equals t.ID_TIPOSOLICITUD
                           join u in db.UNIDAD on f.ID_UNIDAD equals u.ID_UNIDAD
                           select new
                           {
                               f.NOMBRE,
                               f.RUT,
                               u.NOMBRE_UNIDAD,
                               s.FECHA_INICIO,
                               s.FECHA_FIN,
                               t.DESCRIPCION
                           }).Where(f => f.RUT.Equals(rut)).ToList();

            var permisos = new XElement("root");

            foreach (var item in reslinq)
            {
                permisos.Add(
                    new XElement("permiso", new XElement("funcionario",
                new XElement("nombre", item.NOMBRE),
                new XElement("rut", item.RUT),
                new XElement("unidad", item.NOMBRE_UNIDAD)),
            new XElement("detalle", new XElement("fecha_inicio", item.FECHA_INICIO.ToString("dd-MM-yyyy")),
                new XElement("fecha_fin", item.FECHA_FIN.ToString("dd-MM-yyyy")),
                new XElement("descripcion", item.DESCRIPCION)))
                    );
            }
            return new HttpResponseMessage() { Content = new StringContent(string.Concat(permisos), Encoding.UTF8, "application/xml") };
        }

        
    }
}
