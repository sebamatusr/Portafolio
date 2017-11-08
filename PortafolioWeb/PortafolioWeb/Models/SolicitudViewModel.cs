using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PortafolioWeb.Models
{
    [DataContract(IsReference = true)]
    public class SolicitudViewModel : Entities
    {
        public IEnumerable<SOLICITUD> Solicitudes { get; set; }
        public IEnumerable<TIPO_SOLICITUD> Tipos_Solicitud { get; set; }
        public IEnumerable<FUNCIONARIO> Funcionarios { get; set; }

       
        
        public SOLICITUD solicitud { get; set; }

        public IEnumerable<MOTIVO> Motivos { get; set; }


    }
}