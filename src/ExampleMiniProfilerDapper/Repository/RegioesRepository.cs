using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Slapper;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace ExampleMiniProfilerDapper.Repository
{
    public class RegioesRepository : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IDbConnection _conection;

        public RegioesRepository(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration;
            _conection = GetDbConnection();
        }

        private IDbConnection GetDbConnection()
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            if(_environment.IsDevelopment()) return new ProfiledDbConnection(connection, MiniProfiler.Current);

            return connection;
        }

        public IEnumerable<Regiao> GetRegiaos(string? id = null)
        {
            var queryWithParameter = !string.IsNullOrWhiteSpace(id);

            var sqlCmd = "SELECT R.CodRegiao, " +
                    "R.NomeRegiao " +
            "FROM dbo.Regioes R " +
            (queryWithParameter ? $"WHERE (R.CodRegiao = @CodigoRegiao) " : String.Empty) +
            "ORDER BY R.NomeRegiao";

            object? paramQuery = null;
            
            if(queryWithParameter) paramQuery = new {CodigoRegiao = id};

            var dados = _conection.Query<dynamic>(sqlCmd, paramQuery);

            AutoMapper.Configuration.AddIdentifier(typeof(Regiao), "CodRegiao");

            return (AutoMapper.MapDynamic<Regiao>(dados) as IEnumerable<Regiao>).ToArray();
        }

        public void Dispose()
        {
           _conection.Close();
        }
    }
}