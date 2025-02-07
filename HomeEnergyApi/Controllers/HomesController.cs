using Microsoft.AspNetCore.Mvc;
using HomeEnergyApi.Models;

namespace HomeEnergyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomesController : ControllerBase
    {
        private IReadRepository<int, Home> repository;

        public HomesController(IReadRepository<int, Home> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(repository.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            if (id > repository.FindAll().Count)
            {
                return NotFound();
            }
            var home = repository.FindById(id);

            return Ok(home);
        }
    }
}