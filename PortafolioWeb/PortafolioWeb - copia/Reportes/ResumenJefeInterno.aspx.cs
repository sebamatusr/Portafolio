using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PortafolioWeb.Reportes
{
    public partial class ResumenJefeInterno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if(Session["rol_name"].Equals("Jefe Unidad Interior"))
                //{
                    //var id_unidad = Session["id_unidad"].ToString();
                    //SqlDataSource1.SelectParameters["ID_UNIDAD"].DefaultValue = id_unidad;
                //}
                //else
                    //SqlDataSource1.SelectParameters["ID_UNIDAD"].DefaultValue = "26";
               
            }
            catch (Exception)
            {
                SqlDataSource1.SelectParameters["ID_UNIDAD"].DefaultValue = "0";
            }
        }

        protected void id_unidad_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}