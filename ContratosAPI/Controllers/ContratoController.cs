using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContratosAPI.Data;
using ContratosAPI.Models;
using ContratosAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.FeatureManagement;

namespace ContratosAPI.Controllers
{   
    [ApiController]
    [Route("contrato")]
    public class ContratoController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;
        private readonly IMemoryCache _cache;
        private readonly DataContext _context;
        private readonly IContratoService _service;
        public ContratoController(IContratoService service, DataContext context, IMemoryCache cache, IFeatureManager featureManager)
        {
            _context = context;
             _cache = cache;
            _featureManager = featureManager;
            _service = service;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<List<Contrato>> GetContratosCache([FromServices] DataContext context, [FromServices]IMemoryCache cache)
        {
            if (!await _featureManager.IsEnabledAsync(nameof(MyFeatureFlags.CacheRepositories)))
            {
                _cache.Remove(_context.Contratos.ToListAsync());
            }
            return await cache.GetOrCreateAsync(_context.Contratos.ToListAsync(), entry => 
            {
                entry.AbsoluteExpiration = new DateTimeOffset(DateTime.Today.AddDays(1));
                return _service.GetContratosService();
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Contrato> GetContrato(int id)
        {
           return await _service.GetContratoService(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Contrato>> Post([FromServices] DataContext context, [FromBody] Contrato contrato)
        {
            if(ModelState.IsValid)
            {
                return await _service.PostContratoService(contrato);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("editar/{id}")]
        public async Task<Contrato> PutContrato(int id, [FromBody] Contrato contrato)
        {
            return await _service.PutContratoService(id, contrato);
        }

        [HttpDelete]
        [Route("deletar/{id}")]
        public async Task<Contrato> DeleteContrato(int id)
        {
            return await _service.DeleteContratoService(id);
        }
    }
}