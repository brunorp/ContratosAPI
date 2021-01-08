using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ContratosAPI.Data;
using ContratosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratosAPI.Controllers
{   [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("prestacoes")]
    public class PrestacaoController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Prestacao>>> Get([FromServices] DataContext context)
        {
            var prestacao = await context.Prestacoes.ToListAsync();
            return prestacao;
        }
    }
}