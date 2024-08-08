using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ExemploAPI2_Json.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunoController : Controller
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExemploAPI-Json;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        private readonly ILogger<AlunoController> _logger;

        public AlunoController(ILogger<AlunoController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public ActionResult CadastrarAluno([FromBody] Aluno aluno)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "INSERT INTO Aluno (Rm, Nome, Curso) VALUES (@Rm, @Nome, @Curso)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Rm", aluno.Rm);
                command.Parameters.AddWithValue("@Nome", aluno.Nome);
                command.Parameters.AddWithValue("@Curso", aluno.Curso);
                connection.Open();

                int hasRows = command.ExecuteNonQuery();
                if (hasRows > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }

        }

        [HttpGet]
        public IEnumerable<Aluno> GetAlunos()
        {
            List<Aluno> alunos = new List<Aluno>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Aluno";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Aluno aluno = new Aluno
                    {
                        Rm = Convert.ToInt32(reader["Rm"]),
                        Nome = reader["Nome"].ToString(),
                        Curso = reader["Curso"].ToString()
                    };

                    alunos.Add(aluno);
                }

                reader.Close();
            }

            return alunos;
        }

        [HttpGet("{rm}", Name = "ObterAlunoPorId")]

        public ActionResult ObterAlunoPorId(int rm)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Aluno WHERE @Rm = Rm";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Rm", rm);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) 
                {
                    Aluno aluno = new Aluno
                    {
                        Rm = Convert.ToInt32(reader["Rm"]),
                        Nome = reader["Nome"].ToString(),
                        Curso = reader["Curso"].ToString()
                    };

                    reader.Close();

                    return Ok(aluno);
                }

                reader.Close();

                return NotFound();
            }
        }

        [HttpPut("{rm}")]

        public ActionResult AtualizarAluno(int rm, [FromBody] Aluno aluno)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) 
            {
                string query = "UPDATE Aluno SET Nome = @Nome, Curso = @Curso WHERE Rm = @Rm";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nome", aluno.Nome);
                command.Parameters.AddWithValue("@Curso", aluno.Curso);
                command.Parameters.AddWithValue("@Rm", rm);
                
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0) 
                {
                    return Ok(aluno);
                }

                return NotFound();
            }


        }

        [HttpDelete("{rm}")]

        public ActionResult DeletarALuno(int rm) 
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "DELETE * FROM Aluno WHERE Rm = @Rm";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Rm", rm);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok();
                }

                return NotFound();
            }
        }

    }
}


