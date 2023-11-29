using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIPEP3.Models
{
    public class Reserva
    {
         public int IdReserva { get; set; }
        public string? Especialidad { get; set; }
        public DateTime DiaReserva { get; set; }

        // Propiedad de clave foránea
        public int? PacienteId { get; set; } // '?' indica que es opcional (puede ser nulo)

        // Propiedad de navegación
        public Paciente? Paciente { get; set; }

        public int? MedicoId { get; set; } 
        public Medico? Medico { get; set; }
    }
}