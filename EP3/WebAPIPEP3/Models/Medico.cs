using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIPEP3.Models
{
    public class Medico
    {
          public int IdMedico { get; set; }
        public  string? NombreMed { get; set; }
        public  string? ApellidoMed { get; set; }
        public  string? RunMed { get; set; }
        public  string? Eunacom { get; set; }
        public  string? NacionalidadMed { get; set; }
        public  string? EspecialidadMed { get; set; }
        public  string? Especialidad { get; set; }
        public TimeSpan Horarios { get; set; }
        public int TarifaHr { get; set; }

        internal static object FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }


}