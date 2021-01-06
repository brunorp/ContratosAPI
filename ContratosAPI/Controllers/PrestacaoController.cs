using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContratosAPI.Data;
using ContratosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratosAPI.Controllers
{   
    [ApiController]
    [Route("prestacoes")]
    public class PrestacaoController : ControllerBase
    {

        private DataContext _context;
        public PrestacaoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Prestacao>>> Get([FromServices] DataContext context)
        {
            var prestacao = await context.Prestacoes.ToListAsync();
            return prestacao;
        }
    }
}