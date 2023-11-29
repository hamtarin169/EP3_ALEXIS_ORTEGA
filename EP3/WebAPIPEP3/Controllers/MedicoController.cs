using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebAPIPEP3.Models;

namespace WebAPIPEP3.Controllers
// se indica la base por donde se gestionaran las solicitudes dentro de la api con el controler
{
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        //se gestiona la lectura privada dentro del controlador
        private readonly string StringConector;

// se gestiono el metodo conector con mysql
        public MedicoController(IConfiguration config)
        {
            StringConector = config?.GetConnectionString("MySqlConnection") ?? throw new ArgumentNullException(nameof(config));
        }

        // gestionamos el medio por el cual se hara la busque del doctor de forma particular a travez de una ID
     [HttpGet("{idMedico}")]
        public ActionResult<Medico> ObtenerDatosMedico(int idMedico)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "SELECT * FROM Medico WHERE IdMedico = @IdMedico";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdMedico", idMedico);

                        MySqlDataReader lector = comandos.ExecuteReader();

                        if (lector.Read())
                        {
                            var medico = new Medico
                            {
                                IdMedico = lector.GetInt32(0),
                                NombreMed = lector.GetString(1),
                                ApellidoMed = lector.GetString(2),
                                RunMed = lector.GetString(3),
                                Eunacom = lector.GetString(4),
                                NacionalidadMed = lector.GetString(5),
                                EspecialidadMed = lector.GetString(6),
                                Especialidad = lector.GetString(7),
                                Horarios = TimeSpan.Parse(lector.GetString(8)), // Ajustar según el tipo de datos de la base de datos
                                TarifaHr = lector.GetInt32(9),
                            };

                            return Ok(medico);
                        }
                        else
                        {
                            return NotFound(new { mensaje = "Médico no encontrado" });
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

        // Gestionamos el metodo por el cual se enlistaran todos los medicos en el centro de salud
        [HttpGet]
        public IActionResult ListarMedicos()
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    using (MySqlCommand comandos = new MySqlCommand("SELECT * FROM Medico", conecta))
                    {
                        MySqlDataReader lector = comandos.ExecuteReader();

                        var medicos = new List<Medico>();

                        while (lector.Read())
                        {
                            medicos.Add(new Medico
                            {
                                IdMedico = lector.GetInt32(0),
                                NombreMed = lector.GetString(1),
                                ApellidoMed = lector.GetString(2),
                                RunMed = lector.GetString(3),
                                Eunacom = lector.GetString(4),
                                NacionalidadMed = lector.GetString(5),
                                EspecialidadMed = lector.GetString(6),
                                Especialidad = lector.GetString(7),
                                Horarios = TimeSpan.Parse(lector.GetString(8)), // Ajustar según el tipo de datos de la base de datos
                                TarifaHr = lector.GetInt32(9),
                            });
                        }

                        return Ok(medicos);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un código de error en la respuesta HTTP.
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Se gestionara el metodo por el cual podremos ingresar nuevos medicos a traves de sus atributos particulares

        [HttpPost]
        public IActionResult AgregarMedico([FromBody] Medico nuevoMedico)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "INSERT INTO Medico (NombreMed, ApellidoMed, RunMed, Eunacom, NacionalidadMed, EspecialidadMed, Especialidad, Horarios, TarifaHr) " +
                                   "VALUES (@NombreMed, @ApellidoMed, @RunMed, @Eunacom, @NacionalidadMed, @EspecialidadMed, @Especialidad, @Horarios, @TarifaHr)";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@NombreMed", nuevoMedico.NombreMed);
                        comandos.Parameters.AddWithValue("@ApellidoMed", nuevoMedico.ApellidoMed);
                        comandos.Parameters.AddWithValue("@RunMed", nuevoMedico.RunMed);
                        comandos.Parameters.AddWithValue("@Eunacom", nuevoMedico.Eunacom);
                        comandos.Parameters.AddWithValue("@NacionalidadMed", nuevoMedico.NacionalidadMed);
                        comandos.Parameters.AddWithValue("@EspecialidadMed", nuevoMedico.EspecialidadMed);
                        comandos.Parameters.AddWithValue("@Especialidad", nuevoMedico.Especialidad);
                        comandos.Parameters.AddWithValue("@Horarios", nuevoMedico.Horarios.ToString()); // Ajustar según el tipo de datos de la base de datos
                        comandos.Parameters.AddWithValue("@TarifaHr", nuevoMedico.TarifaHr);

                        comandos.ExecuteNonQuery();
                    }
                }

                return StatusCode(201, "Médico agregado con éxito");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un código de error en la respuesta HTTP.
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
    }

    //Se gestionara el metodo por el cual se requiere Eliminar a un medico de la base de datos
    [HttpDelete("{idMedico}")]
public IActionResult EliminarMedico(int idMedico)
{
    try
    {
        using (MySqlConnection conecta = new MySqlConnection(StringConector))
        {
            conecta.Open();

            string query = "DELETE FROM Medico WHERE idMedico = @IdMedico";

            using (MySqlCommand comandos = new MySqlCommand(query, conecta))
            {
                comandos.Parameters.AddWithValue("@IdMedico", idMedico);

                int filasAfectadas = comandos.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    return Ok($"Médico con ID {idMedico} eliminado exitosamente");
                }
                else
                {
                    return NotFound($"Médico con ID {idMedico} no encontrado");
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

//Se gestiona el metodo por el cual se podran actualizar datos de los medicos los cuales ya existen en la base de datos

[HttpPut("{idMedico}")]
public IActionResult ActualizarMedico(int idMedico, [FromBody] Medico medicoActualizado)
{
    try
    {
        using (MySqlConnection conecta = new MySqlConnection(StringConector))
        {
            conecta.Open();

            string query = "UPDATE Medico SET NombreMed = @NombreMed, ApellidoMed = @ApellidoMed, RunMed = @RunMed, " +
                           "Eunacom = @Eunacom, NacionalidadMed = @NacionalidadMed, EspecialidadMed = @EspecialidadMed, " +
                           "Especialidad = @Especialidad, Horarios = @Horarios, TarifaHr = @TarifaHr " +
                           "WHERE idMedico = @IdMedico";

            using (MySqlCommand comandos = new MySqlCommand(query, conecta))
            {
                comandos.Parameters.AddWithValue("@IdMedico", idMedico);
                comandos.Parameters.AddWithValue("@NombreMed", medicoActualizado.NombreMed);
                comandos.Parameters.AddWithValue("@ApellidoMed", medicoActualizado.ApellidoMed);
                comandos.Parameters.AddWithValue("@RunMed", medicoActualizado.RunMed);
                comandos.Parameters.AddWithValue("@Eunacom", medicoActualizado.Eunacom);
                comandos.Parameters.AddWithValue("@NacionalidadMed", medicoActualizado.NacionalidadMed);
                comandos.Parameters.AddWithValue("@EspecialidadMed", medicoActualizado.EspecialidadMed);
                comandos.Parameters.AddWithValue("@Especialidad", medicoActualizado.Especialidad);
                comandos.Parameters.AddWithValue("@Horarios", medicoActualizado.Horarios.ToString()); // Ajustar según el tipo de datos de la base de datos
                comandos.Parameters.AddWithValue("@TarifaHr", medicoActualizado.TarifaHr);

                int filasAfectadas = comandos.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    return Ok($"Médico con ID {idMedico} actualizado exitosamente");
                }
                else
                {
                    return NotFound($"Médico con ID {idMedico} no encontrado");
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