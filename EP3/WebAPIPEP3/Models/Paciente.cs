using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIPEP3.Models
{
    public class Paciente
    {
         public int IdPaciente { get; set; }
        public string? NombrePac { get; set; }
        public string? ApellidoPac { get; set; }
        public string? RunPac { get; set; }
        public string? Nacionalidad { get; set; }
        public string? Visa { get; set; }
        public string? Genero { get; set; }
        public string? SintomasPac { get; set; }

        // Propiedad de clave foránea
        public int? MedicoId { get; set; } // '?' indica que es opcional (puede ser nulo)

        // Propiedad de navegación
        public Medico? Medico { get; set; }
    }
}