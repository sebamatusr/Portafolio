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
    
    public partial class TIPO_SOLICITUD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_SOLICITUD()
        {
            this.MOTIVO = new HashSet<MOTIVO>();
            this.SOLICITUD = new HashSet<SOLICITUD>();
        }
        
        public decimal ID_TIPOSOLICITUD { get; set; }
        public string DESCRIPCION { get; set; }
        public decimal CANTIDAD_DIAS { get; set; }
        public System.DateTime FECHA_CREACION { get; set; }
        public System.DateTime FECHA_MODIFACION { get; set; }
        public decimal ESTADO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MOTIVO> MOTIVO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLICITUD> SOLICITUD { get; set; }
    }
}
