using ExampleMiniProfilerDapper.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ExampleMiniProfilerDapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegioesController : ControllerBase
    {
        private readonly ILogger<RegioesController> _logger;
        private readonly RegioesRepository _repository;
        public RegioesController(ILogger<RegioesController> logger, RegioesRepository repository )
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Regiao> Get()
        {
            using(_repository)
            {
                var regioes = _repository.GetRegiaos();

                _logger.LogInformation($"{nameof(Get)}: {regioes.Count()} registro(s) encontrado(s)");

                return regioes;
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Regiao> GetbyId(string id)
        {
            using(_repository)
            {
                var regioes = _repository.GetRegiaos(id).SingleOrDefault();
                
                _logger.LogInformation($"{nameof(Get)}: {regioes?.NomeRegiao.Count() ?? 0} registro(s) encontrado(s)");

                if(regioes is null) return NotFound();

                return regioes;
            }
        }
    }
}