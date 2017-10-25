using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Oracle.ManagedDataAccess.Client;


namespace WebServiceRH
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IAsistencia
    {

        public bool GetAsistencia(string rut, DateTime fecha)
        {
            //
            using (OracleConnection con = new OracleConnection())
            {
                string constring = "DATA SOURCE=190.161.202.171:1521/DBORACLE;PASSWORD=portafolio;PERSIST SECURITY INFO=True;USER ID=VISTAHERMOSA";

                con.ConnectionString = constring;

                con.Open();

                OracleCommand command = con.CreateCommand();

                string sql;

                string formato = fecha.ToString("dd-MM-yyyy");

                sql = "SELECT ASISTE FROM ASISTENCIA WHERE RUT = '" + rut + "' AND to_char(FECHA, 'DD-MM-YYYY') = '" + formato + "'";

                command.CommandText = sql;

                OracleDataReader reader = command.ExecuteReader();

                string asiste = "0";

                while (reader.Read())
                {
                    asiste = reader["asiste"].ToString();
                }
                
                //reader.Close(); 

                return asiste == "1";
            }
        }

        

        public int GetAntiguedad(string rut)
        {
            //Obtiene la antigüedad del funcionario en meses
            using (OracleConnection con = new OracleConnection())
            {
                string constring = "DATA SOURCE=190.161.202.171:1521/DBORACLE;PASSWORD=portafolio;PERSIST SECURITY INFO=True;USER ID=VISTAHERMOSA";
                con.ConnectionString = constring;
                con.Open();
                OracleCommand command = con.CreateCommand();
                string sql;
                sql = "SELECt TRUNC(MONTHS_BETWEEN(SYSDATE, FECHA_INGRESO)) from CONTRATO_FUNCIONARIO where rut = '" + rut + "'";
                command.CommandText = sql;
                OracleDataReader reader = command.ExecuteReader();
                reader.Read();

                int result = Convert.ToInt32(reader.GetValue(0));

                reader.Close();

                return result;
              
            }
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
