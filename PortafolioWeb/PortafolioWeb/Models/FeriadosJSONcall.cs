using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PortafolioWeb.Models
{
    public class FeriadosJSONcall
    {
        public static Root GetFeriados()
        {
            string apiurl = "https://feriados-cl-api.herokuapp.com/db";

            using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                string json = client.DownloadString(apiurl);

                Root items = JsonConvert.DeserializeObject<Root>(json);

                return items;
            }

        }


    }
}