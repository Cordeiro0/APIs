using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Dynamic;

namespace ExemploAPI2_Json.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExemploAPI-Json;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<bool> ValidarLogin([FromBody] Login login)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Login WHERE Rm = @Rm AND Senha = @Senha";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Rm", login.Rm);
                command.Parameters.AddWithValue("@Senha", login.Senha);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return Ok(true);
                }

                return BadRequest(false);
            }
        }
    }
}
