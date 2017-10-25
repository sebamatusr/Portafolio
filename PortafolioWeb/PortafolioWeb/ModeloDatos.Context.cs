﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ESTADO> ESTADO { get; set; }
        public virtual DbSet<FUNCIONARIO> FUNCIONARIO { get; set; }
        public virtual DbSet<HISTORIAL_RESOLUCIONES> HISTORIAL_RESOLUCIONES { get; set; }
        public virtual DbSet<MOTIVO> MOTIVO { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<SOLICITUD> SOLICITUD { get; set; }
        public virtual DbSet<TIPO_SOLICITUD> TIPO_SOLICITUD { get; set; }
        public virtual DbSet<UNIDAD> UNIDAD { get; set; }

        public virtual int LoginValidador(string vRUT, string vPASS)
        {
            //var vRUTParameter = new OracleParameter("VRUT", OracleDbType.Varchar2, "vRUT", ParameterDirection.Input);
            //var vPASSParameter = new OracleParameter("VPASS", OracleDbType.Varchar2, "vPASS", ParameterDirection.Input);
            //var vSALIDAParameter = new OracleParameter("VSALIDA", OracleDbType.Int32, ParameterDirection.Output);

            var vSALIDA = new ObjectParameter("VSALIDA", typeof(int));

            var vRUTParameter = vRUT != null ?
                new ObjectParameter("VRUT", vRUT) :
                new ObjectParameter("VRUT", typeof(string));

            var vPASSParameter = vPASS != null ?
                new ObjectParameter("VPASS", vPASS) :
                new ObjectParameter("VPASS", typeof(string));

            var query = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("LoginValidador", vRUTParameter, vPASSParameter, vSALIDA);

            int output = Convert.ToInt32(vSALIDA.Value);

            return output;
        }
    }
}
