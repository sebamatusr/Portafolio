//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortafolioWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class SOLICITUD
    {
        public decimal ID_SOLICITUD { get; set; }
        public decimal CODIGO_VERIFICACION { get; set; }
        public System.DateTime FECHA_INICIO { get; set; }
        public System.DateTime FECHA_FIN { get; set; }
        public System.DateTime FECHA_CREACION { get; set; }
        public System.DateTime FECHA_MODIFICACION { get; set; }
        public decimal ESTADO { get; set; }
        public decimal ID_TIPOSOLICITUD { get; set; }
        public decimal ID_ESTADO { get; set; }
        public string RUT_FUNCIONARIO { get; set; }
        public string RUT_EVALUADOR { get; set; }
    
        public virtual ESTADO ESTADO1 { get; set; }
        public virtual FUNCIONARIO FUNCIONARIO { get; set; }
        public virtual FUNCIONARIO FUNCIONARIO1 { get; set; }
        public virtual TIPO_SOLICITUD TIPO_SOLICITUD { get; set; }
    }
}
