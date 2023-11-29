using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebAPIPEP3.Models;

namespace WebAPIPEP3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly string StringConector;

        public PacienteController(IConfiguration config)
        {
            StringConector = config?.GetConnectionString("MySqlConnection") ?? throw new ArgumentNullException(nameof(config));
        }

  [HttpGet("{idPaciente}")]
public IActionResult ObtenerPacientePorId(int idPaciente)
{
    try
    {
        using (MySqlConnection conecta = new MySqlConnection(StringConector))
        {
            conecta.Open();

            string query = "SELECT * FROM Paciente WHERE IdPaciente = @IdPaciente";

            using (MySqlCommand comandos = new MySqlCommand(query, conecta))
            {
                comandos.Parameters.AddWithValue("@IdPaciente", idPaciente);

                MySqlDataReader lector = comandos.ExecuteReader();

                if (lector.Read())
                {
                    Paciente paciente = new Paciente
                    {
                        IdPaciente = lector.GetInt32(0),
                        NombrePac = lector.GetString(1),
                        ApellidoPac = lector.GetString(2),
                        RunPac = lector.GetString(3),
                        Nacionalidad = lector.GetString(4),
                        Visa = lector.GetString(5),
                        Genero = lector.GetString(6),
                        SintomasPac = lector.GetString(7),
                        MedicoId = lector.IsDBNull(8) ? (int?)null : lector.GetInt32(8),
                    };

                    return Ok(paciente);
                }
                else
                {
                    return NotFound($"Paciente con ID {idPaciente} no encontrado");
                }
            
        }
    }}
    catch (Exception ex)
    {
        // Manejar cualquier excepción y devolver un código de error en la respuesta HTTP.
        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
    }
}


        [HttpGet]
        public IActionResult ListarPacientes()
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    using (MySqlCommand comandos = new MySqlCommand("SELECT * FROM Paciente", conecta))
                    {
                        MySqlDataReader lector = comandos.ExecuteReader();

                        var pacientes = new List<Paciente>();

                        while (lector.Read())
                        {
                            pacientes.Add(new Paciente
                            {
                             IdPaciente = lector.GetInt32(0),
                            NombrePac = lector.GetString(1),
                            ApellidoPac = lector.GetString(2),
                            RunPac = lector.GetString(3),
                            Nacionalidad = lector.GetString(4),
                            Visa = lector.GetString(5),
                            Genero = lector.GetString(6),
                            SintomasPac = lector.GetString(7),
                            MedicoId = lector.IsDBNull(8) ? (int?)null : lector.GetInt32(8), // Manejar el caso de clave foránea opcional (puede ser nula)
                            });
                        }

                        return Ok(pacientes);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un código de error en la respuesta HTTP.
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

      


        [HttpPost]
        public IActionResult AgregarPaciente([FromBody] Paciente nuevoPaciente)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "INSERT INTO Paciente (NombrePac, ApellidoPac, RutPac, EdadPac, Diagnostico) " +
                                   "VALUES (@NombrePac, @ApellidoPac, @RutPac, @EdadPac, @Diagnostico)";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                      comandos.Parameters.AddWithValue("@NombrePac", nuevoPaciente.NombrePac);
                comandos.Parameters.AddWithValue("@ApellidoPac", nuevoPaciente.ApellidoPac);
                comandos.Parameters.AddWithValue("@RunPac", nuevoPaciente.RunPac);
                comandos.Parameters.AddWithValue("@Nacionalidad", nuevoPaciente.Nacionalidad);
                comandos.Parameters.AddWithValue("@Visa", nuevoPaciente.Visa);
                comandos.Parameters.AddWithValue("@Genero", nuevoPaciente.Genero);
                comandos.Parameters.AddWithValue("@SintomasPac", nuevoPaciente.SintomasPac);
                comandos.Parameters.AddWithValue("@MedicoId", nuevoPaciente.MedicoId ?? (object)DBNull.Value); // Manejar el caso de clave foránea opcional (puede ser nula)

                comandos.ExecuteNonQuery();
                    }
                }

                return StatusCode(201, "Paciente agregado con éxito");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un código de error en la respuesta HTTP.
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{idPaciente}")]
        public IActionResult EliminarPaciente(int idPaciente)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "DELETE FROM Paciente WHERE idPaciente = @IdPaciente";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdPaciente", idPaciente);

                        int filasAfectadas = comandos.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            return Ok($"Paciente con ID {idPaciente} eliminado exitosamente");
                        }
                        else
                        {
                            return NotFound($"Paciente con ID {idPaciente} no encontrado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un código de error en la respuesta HTTP.
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{idPaciente}")]
        public IActionResult ActualizarPaciente(int idPaciente, [FromBody] Paciente pacienteActualizado)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "UPDATE Paciente SET NombrePac = @NombrePac, ApellidoPac = @ApellidoPac, " +
                                   "RutPac = @RutPac, EdadPac = @EdadPac, Diagnostico = @Diagnostico " +
                                   "WHERE idPaciente = @IdPaciente";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                comandos.Parameters.AddWithValue("@IdPaciente", idPaciente);
                comandos.Parameters.AddWithValue("@NombrePac", pacienteActualizado.NombrePac);
                comandos.Parameters.AddWithValue("@ApellidoPac", pacienteActualizado.ApellidoPac);
                comandos.Parameters.AddWithValue("@RunPac", pacienteActualizado.RunPac);
                comandos.Parameters.AddWithValue("@Nacionalidad", pacienteActualizado.Nacionalidad);
                comandos.Parameters.AddWithValue("@Visa", pacienteActualizado.Visa);
                comandos.Parameters.AddWithValue("@Genero", pacienteActualizado.Genero);
                comandos.Parameters.AddWithValue("@SintomasPac", pacienteActualizado.SintomasPac);
                comandos.Parameters.AddWithValue("@MedicoId", pacienteActualizado.MedicoId ?? (object)DBNull.Value); // Manejar el caso de clave foránea opcional (puede ser nula)

                        int filasAfectadas = comandos.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            return Ok($"Paciente con ID {idPaciente} actualizado exitosamente");
                        }
                        else
                        {
                            return NotFound($"Paciente con ID {idPaciente} no encontrado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un código de error en la respuesta HTTP.
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}