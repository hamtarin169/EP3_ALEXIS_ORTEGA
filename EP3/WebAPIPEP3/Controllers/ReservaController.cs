using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebAPIPEP3.Models;

namespace WebAPIPEP3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
   {
        private readonly string StringConector;

        public ReservaController(IConfiguration config)
        {
            StringConector = config?.GetConnectionString("MySqlConnection") ?? throw new ArgumentNullException(nameof(config));
        }




        [HttpGet]
        public IActionResult ListarReservas()
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    using (MySqlCommand comandos = new MySqlCommand("SELECT * FROM Reserva", conecta))
                    {
                        MySqlDataReader lector = comandos.ExecuteReader();

                        var reservas = new List<Reserva>();

                        while (lector.Read())
                        {
                            reservas.Add(new Reserva
                            {
                               IdReserva = lector.GetInt32(0),
                            Especialidad = lector.GetString(1),
                            DiaReserva = lector.GetDateTime(2),
                            PacienteId = lector.IsDBNull(3) ? (int?)null : lector.GetInt32(3), // Manejar el caso de clave foránea opcional (puede ser nula)
                            });
                        }

                        return Ok(reservas);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{idReserva}")]
        public IActionResult ObtenerReserva(int idReserva)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "SELECT * FROM Reserva WHERE IdReserva = @IdReserva";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdReserva", idReserva);

                        MySqlDataReader lector = comandos.ExecuteReader();

                        if (lector.Read())
                        {
                            var reserva = new Reserva
                            {
                            IdReserva = lector.GetInt32(0),
                            Especialidad = lector.GetString(1),
                            DiaReserva = lector.GetDateTime(2),
                            PacienteId = lector.IsDBNull(3) ? (int?)null : lector.GetInt32(3), // Manejar el caso de clave foránea opcional (puede ser nula)
                            MedicoId = lector.IsDBNull(4) ? (int?)null : lector.GetInt32(4), // Manejar el caso de clave foránea opcional (puede ser nula)nt?)null : lector.GetInt32(3), // Manejar el caso de clave foránea opcional (puede ser nula)
                            };

                            return Ok(reserva);
                        }
                        else
                        {
                            return NotFound($"Reserva con ID {idReserva} no encontrado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AgregarReserva([FromBody] Reserva nuevaReserva)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "INSERT INTO Reserva (FechaReserva, Observaciones, PacienteId, MedicoId) " +
                                   "VALUES (@FechaReserva, @Observaciones, @PacienteId, @MedicoId)";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                       
                    comandos.Parameters.AddWithValue("@Especialidad", nuevaReserva.Especialidad);
                    comandos.Parameters.AddWithValue("@Especialidad", nuevaReserva.Especialidad);
                    comandos.Parameters.AddWithValue("@DiaReserva", nuevaReserva.DiaReserva);
                    comandos.Parameters.AddWithValue("@PacienteId", nuevaReserva.PacienteId ?? (object)DBNull.Value); // Manejar el caso de clave foránea opcional (puede ser nula)
                    comandos.Parameters.AddWithValue("@MedicoId", nuevaReserva.MedicoId ?? (object)DBNull.Value); // Manejar el caso de clave foránea opcional (puede ser nula)

comandos.ExecuteNonQuery();
comandos.ExecuteNonQuery();
                    }
                }

                return StatusCode(201, "Reserva agregada con éxito");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{idReserva}")]
        public IActionResult EliminarReserva(int idReserva)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "DELETE FROM Reserva WHERE IdReserva = @IdReserva";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdReserva", idReserva);

                        int filasAfectadas = comandos.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            return Ok($"Reserva con ID {idReserva} eliminada exitosamente");
                        }
                        else
                        {
                            return NotFound($"Reserva con ID {idReserva} no encontrada");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{idReserva}")]
        public IActionResult ActualizarReserva(int idReserva, [FromBody] Reserva reservaActualizada)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "UPDATE Reserva SET FechaReserva = @FechaReserva, " +
                                   "Observaciones = @Observaciones, PacienteId = @PacienteId, " +
                                   "MedicoId = @MedicoId WHERE IdReserva = @IdReserva";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdReserva", idReserva);
                        comandos.Parameters.AddWithValue("@Especialidad", reservaActualizada.Especialidad);
                        comandos.Parameters.AddWithValue("@DiaReserva", reservaActualizada.DiaReserva);
                        comandos.Parameters.AddWithValue("@PacienteId", reservaActualizada.PacienteId ?? (object)DBNull.Value); // Manejar el caso de clave foránea opcional (puede ser nula)
                        comandos.Parameters.AddWithValue("@MedicoId", reservaActualizada.MedicoId ?? (object)DBNull.Value); // Manejar el caso de clave foránea opcional (puede ser nula)
                        int filasAfectadas = comandos.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            return Ok($"Reserva con ID {idReserva} actualizada exitosamente");
                        }
                        else
                        {
                            return NotFound($"Reserva con ID {idReserva} no encontrada");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
   }
}
   