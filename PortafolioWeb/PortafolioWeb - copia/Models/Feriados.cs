using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortafolioWeb.Models
{
    public class Feriados
    {
        public string date { get; set; }
        public string title { get; set; }
        public string extra { get; set; }

    }
    public class Root
    {
        public Feriados[] feriados { get; set; }
    }
}