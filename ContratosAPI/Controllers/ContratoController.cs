using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContratosAPI.Data;
using ContratosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratosAPI.Controllers
{   
    [ApiController]
    [Route("contrato")]
    public class ContratoController : ControllerBase
    {

        private DataContext _context;
        public ContratoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Contrato>>> GetContratos([FromServices] DataContext context)
        {
            var contratos = await context.Contratos.ToListAsync();
            return contratos;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Contrato>> GetContrato(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);

            if(contrato == null)
            {
                return NotFound();
            }

            return contrato;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Contrato>> Post([FromServices] DataContext context, [FromBody] Contrato contrato)
        {
           
            if(ModelState.IsValid)
            {
                context.Contratos.Add(contrato);
                PostPrestacao(contrato, contrato.Id);
                await context.SaveChangesAsync();
                return contrato;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("editar/{id}")]
        public async Task<ActionResult> PutContrato(int id, [FromBody] Contrato contrato)
        {
            if(contrato == null)
            {
                return NotFound();
            }

            var contratoExistente = await _context.Contratos.FindAsync(id);
            contratoExistente.DataContratacao = contrato.DataContratacao;
            contratoExistente.QuantidadeParcelas = contrato.QuantidadeParcelas;
            contratoExistente.ValorFinanciado = contrato.ValorFinanciado;
            contratoExistente.Prestacoes = contrato.Prestacoes;

            var prestacoes =  await _context.Prestacoes.Where(p => id == p.ContratoId).ToArrayAsync();
            _context.Prestacoes.RemoveRange(prestacoes);
            PostPrestacao(contrato, id);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("deletar/{id}")]
        public async Task<ActionResult<Contrato>> DeleteContrato(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);

            if(contrato == null)
            {
                return NotFound();
            }

            _context.Contratos.Remove(contrato);
            var prestacoes = await _context.Prestacoes.Where(p => id == p.ContratoId).ToArrayAsync();
            _context.Prestacoes.RemoveRange(prestacoes);

            await _context.SaveChangesAsync();

            return contrato;
        }

        private void PostPrestacao(Contrato contrato, int id){
            var dataVencimento = DateTime.Today.Date.AddDays(30);
            var dataPagamento = DateTime.Today.Date.AddDays(25);
            double valorPrestacao = (double)contrato.ValorFinanciado/contrato.QuantidadeParcelas;
            var status = "Baixada";
            for(var i = 0; i<contrato.QuantidadeParcelas; i++)
            {
                Prestacao prestacao = new Prestacao();
                prestacao.ContratoId = id;
                prestacao.DataVencimento = dataVencimento;
                prestacao.DataPagamento = dataPagamento;
                prestacao.Valor = valorPrestacao;
                    if(prestacao.DataVencimento >= DateTime.Today.Date && prestacao.DataPagamento.Equals(null))
                    status = "Aberta";
                else if(prestacao.DataVencimento < DateTime.Today.Date && prestacao.DataPagamento.Equals(null))
                    status = "Atrasada";
                prestacao.Status = status;
                _context.Prestacoes.Add(prestacao);
                dataVencimento = dataVencimento.AddDays(30);
                dataPagamento = dataPagamento.AddDays(25);
            }
        }

    }
}