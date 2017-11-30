using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortafolioWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;
using System.Globalization;

namespace PortafolioWeb.Controllers
{
    public class JefeInternoController : Controller
    {
        private Entities db = new Entities();
        // GET: JefeInterno
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConsultarPermisosFuncionarios()
        {
            return View();
        }
        public ActionResult GetPermisosPorUnidad(int unidad)
        {

            db.Configuration.LazyLoadingEnabled = false;
            SolicitudViewModel model = new SolicitudViewModel();
            var solicitudes = db.SOLICITUD.Where(c => c.FUNCIONARIO.ID_UNIDAD.Equals(unidad)).ToList();
            object jsonO = new { data = solicitudes };
            var json = JsonConvert.SerializeObject(solicitudes, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
            //JObject jsonObj = JObject.Parse(json);

            return Content(json, "application/json");
        }
        // GET: JefeInterno/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: JefeInterno/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JefeInterno/Create
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

        // GET: JefeInterno/Edit/5
        public ActionResult Evaluar(int id)
        {
            using (db)
            {
                SolicitudViewModel model = new SolicitudViewModel();

                model.Solicitudes = db.SOLICITUD.ToList();
                model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();
                model.solicitud = model.SOLICITUD.Where(a => a.ID_SOLICITUD == id).FirstOrDefault();
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Evaluar(FormCollection coll)
        {
            try
            {
                bool status = false;
                if (Convert.ToInt32(coll["solicitud.ID_SOLICITUD"]) > 0)
                {
                    int idsol = Convert.ToInt32(coll["solicitud.ID_SOLICITUD"]);
                    SOLICITUD permiso = db.SOLICITUD.Where(a => a.ID_SOLICITUD == idsol).FirstOrDefault();
                    if (permiso != null)
                    {

                        permiso.RUT_EVALUADOR = Session["rut"].ToString();
                        permiso.FECHA_MODIFICACION = DateTime.Now;
                        permiso.ID_ESTADO = 11;

                        db.Configuration.AutoDetectChangesEnabled = true;
                        db.ChangeTracker.HasChanges();
                        db.SaveChanges();
                        status = true;

                        MemoryStream memoryStream = new MemoryStream();

                        MemoryStream pdfStream = GenerarDocumentoPDF(permiso, memoryStream);

                        NetMailController mailController = new NetMailController();

                        permiso.FUNCIONARIO = db.FUNCIONARIO.Where(f => f.RUT.Equals(permiso.RUT_FUNCIONARIO)).FirstOrDefault();

                        mailController.sendMail(permiso.FUNCIONARIO.EMAIL, permiso.FUNCIONARIO.NOMBRE, pdfStream);
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

        public void VerPDF(int id_permiso)
        {
            SOLICITUD permiso = db.SOLICITUD.Where(a => a.ID_SOLICITUD == id_permiso).FirstOrDefault();
            MemoryStream memoryStream = new MemoryStream();
            MemoryStream pdfStream = GenerarDocumentoPDF(permiso, memoryStream);
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "inline; filename=" + "Permiso.pdf");
            //Response.AddHeader("Content-Length", docSize.ToString());
            Response.BinaryWrite((byte[])pdfStream.ToArray());
            Response.End();

        }
        public void ResendMail(int id_permiso)
        {
            SOLICITUD permiso = db.SOLICITUD.Where(a => a.ID_SOLICITUD == id_permiso).FirstOrDefault();
            MemoryStream memoryStream = new MemoryStream();
            MemoryStream pdfStream = GenerarDocumentoPDF(permiso, memoryStream);
            NetMailController mailController = new NetMailController();
            permiso.FUNCIONARIO = db.FUNCIONARIO.Where(f => f.RUT.Equals(permiso.RUT_FUNCIONARIO)).FirstOrDefault();
            mailController.sendMail(permiso.FUNCIONARIO.EMAIL, permiso.FUNCIONARIO.NOMBRE, pdfStream);

        }

        private MemoryStream GenerarDocumentoPDF(SOLICITUD permiso, MemoryStream memstream)
        {
            FUNCIONARIO func = db.FUNCIONARIO.Where(r => r.RUT.Equals(permiso.RUT_FUNCIONARIO)).FirstOrDefault();
            UNIDAD unidadfunc = db.UNIDAD.Where(u => u.ID_UNIDAD == func.ID_UNIDAD).FirstOrDefault();
            string rutsession = Session["rut"].ToString();
            FUNCIONARIO revisor = db.FUNCIONARIO.Where(r => r.RUT.Equals(rutsession)).FirstOrDefault();

            string nombrefunc = func.NOMBRE.ToString() + " " + func.APELLIDO_PATERNO.ToString() + " " + func.APELLIDO_MATERNO.ToString();
            string nombrerevisor = revisor.NOMBRE.ToString() + " " + revisor.APELLIDO_PATERNO.ToString() + " " + revisor.APELLIDO_MATERNO.ToString();
            revisor.ROL = db.ROL.Where(r => r.ID_ROL == revisor.ID_ROL).FirstOrDefault();
            string rolrevisor = revisor.ROL.DESCRIPCION_ROL.ToUpper();

            memstream.Position = 0;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, memstream);
            doc.Open();

            string imgurl = "C:/Users/Mr Eko/Source/Repos/Portafolio/PortafolioWeb/PortafolioWeb/Content/img/logoMunicipalidad.PNG";
            iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imgurl);
            png.ScaleToFit(150f, 150f);
            png.Alignment = Element.ALIGN_RIGHT;
            doc.Add(png);

            string fechastr = DateTime.Now.ToLongDateString();
            Paragraph fecha = new Paragraph(fechastr);
            fecha.Alignment = Element.ALIGN_RIGHT;

            doc.Add(fecha);
            Chunk glue = new Chunk(new VerticalPositionMark());
            Paragraph p = new Paragraph("ESTIMADO(A)");
            p.Add(new Paragraph(nombrefunc.ToUpper()));
            p.Add(new Paragraph("I.MUNICIPALIDAD DE VISTA HERMOSA"));
            p.Add(new Paragraph("PRESENTE"));
            p.Add(new Paragraph(" "));
            doc.Add(p);

            doc.Add(new Paragraph(nombrerevisor.ToUpper()));
            doc.Add(new Paragraph(rolrevisor));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("CONSIDERANDO:"));
            doc.Add(new Paragraph("1.- LA SOLICITUD DE PERMISO ADMINISTRATIVO CON GOCE DE REMUNERACIONES, PRESENTADA POR DON(A) " + nombrefunc.ToUpper() +", FUNCIONARIO DE LA UNIDAD " + unidadfunc.NOMBRE_UNIDAD.ToUpper() +"." ));
            doc.Add(new Paragraph("2.- LO DISPUESTO EN LA LEY 18.883 DE 1989, SOBRE ESTATUTO ADMINISTRATIVO."));
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("DECRETO:"));
            
            Root feriados = FeriadosJSONcall.GetFeriados();
            int dias = DateTimeExtention.BusinessDaysUntil(permiso.FECHA_INICIO, permiso.FECHA_FIN, feriados.feriados); 

            doc.Add(new Paragraph("1.- AUTORIZO " + dias + " DÍAS DE PERMISO ADMINISTRATIVO CON GOCE DE REMUNERACIONES, DESDE EL " + permiso.FECHA_INICIO.ToLongDateString().ToUpper() + " HASTA EL "+ permiso.FECHA_FIN.ToLongDateString().ToUpper() + " A DON(A) " + nombrefunc.ToUpper() + "."));

            JsonResult jsondiasrestantes = new FuncionarioSolicitudController().GetPermisos(Convert.ToInt32(permiso.ID_TIPOSOLICITUD), func.RUT.ToString());
            int diasrestantes = Convert.ToInt32(jsondiasrestantes.Data) - dias;

            doc.Add(new Paragraph("2.- DECLARSE QUE LUEGO DE LA UTILIZACIÓN DE ESTE PERMISO, LE QUEDARÁN " + diasrestantes + " DÍAS DE PERMISO ADMINISTRATIVO CORRESPONDIENTE AL AÑO " + DateTime.Now.Year.ToString().ToUpper()));
            doc.Add(new Paragraph(" "));
            Paragraph anotese = new Paragraph("ANÓTESE, COMUNÍQUESE Y ARCHÍVESE");
            anotese.Alignment = Element.ALIGN_CENTER;
            doc.Add(anotese);
            doc.Add(new Paragraph(" "));

            string firmaurl = "C:/Users/Mr Eko/Source/Repos/Portafolio/PortafolioWeb/PortafolioWeb/Content/img/firma.PNG";
            iTextSharp.text.Image firmapng = iTextSharp.text.Image.GetInstance(firmaurl);
            firmapng.ScaleToFit(125f, 125f);
            firmapng.Alignment = Element.ALIGN_RIGHT;
            doc.Add(firmapng);

            Paragraph jefeinterno = new Paragraph(nombrerevisor.ToUpper());
            jefeinterno.Alignment = Element.ALIGN_RIGHT;
            Paragraph roljefeinter = new Paragraph(rolrevisor.ToUpper());
            roljefeinter.Alignment = Element.ALIGN_RIGHT;

            string codigover = permiso.CODIGO_VERIFICACION;
            Paragraph codigo = new Paragraph("código de verificación del documento: " + codigover);

            codigo.Alignment = Element.ALIGN_CENTER;
            

            doc.Add(jefeinterno);
            doc.Add(roljefeinter);

            doc.Add(new Paragraph(" "));
            doc.Add(codigo);
            wri.CloseStream = false;
            doc.Close();

            return memstream;
        }

        public ActionResult Rechazar(int id)
        {
            using (db)
            {
                SolicitudViewModel model = new SolicitudViewModel();

                model.Solicitudes = db.SOLICITUD.ToList();
                model.Tipos_Solicitud = db.TIPO_SOLICITUD.ToList();
                model.solicitud = model.SOLICITUD.Where(a => a.ID_SOLICITUD == id).FirstOrDefault();
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Rechazar(FormCollection coll)
        {
            try
            {
                bool status = false;
                if (Convert.ToInt32(coll["solicitud.ID_SOLICITUD"]) > 0)
                {
                    int idsol = Convert.ToInt32(coll["solicitud.ID_SOLICITUD"]);
                    var permiso = db.SOLICITUD.Where(a => a.ID_SOLICITUD == idsol).FirstOrDefault();
                    if (permiso != null)
                    {
                        permiso.FECHA_MODIFICACION = DateTime.Now;
                        permiso.ID_ESTADO = 12;

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
        // GET: JefeInterno/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JefeInterno/Delete/5
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
        public ActionResult ValidarPermiso()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ValidarPermiso (FormCollection data)
        {
            string codigo = data["codigo"].ToString();
            bool resultado = ValidarSolicitud(codigo);
            if (resultado)
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
        public ActionResult ResumenCuantitativo()
        {
            return View();
        }
    }
}
