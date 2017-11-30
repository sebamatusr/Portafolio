using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace PortafolioWeb.Reportes
{
    public partial class ResumenCuantitativo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ReportViewer1.ProcessingMode = ProcessingMode.Local;

            //DataSetResumenCuantitativo.SolicitudesResumenDataTable dataset = new DataSetResumenCuantitativo().SolicitudesResumen;
            //ReportDataSource rds = new ReportDataSource("resumendata", dataset.DataSet.Tables[0]);   
            //rds.Name = "resumendataset";
            //rds.Value = dataset.DataSet.Tables[0];

            //ReportViewer1.LocalReport.DataSources.Clear();
            //ReportViewer1.LocalReport.DataSources.Add(rds);
            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReporteCuantitativo.rdlc");

            string unidad = Request.QueryString["unidad"];
            //SqlDataSource1.SelectParameters["IDUNIDAD"].DefaultValue = unidad;
            ReportViewer1.LocalReport.Refresh();
        }
    }
}