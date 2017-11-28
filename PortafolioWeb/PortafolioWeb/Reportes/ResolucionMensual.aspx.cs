using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortafolioWeb.Reportes
{
    public partial class ResolucionMensual : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string unidad = Request.QueryString["unidad"];

            SqlDataSource1.SelectParameters["IDUNIDAD"].DefaultValue = unidad;
        }
    }
}