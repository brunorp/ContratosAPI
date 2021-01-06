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
        public async Task<ActionResult<List<Contrato>>> Get([FromServices] DataContext context)
        {
            var contratos = await context.Contratos.ToListAsync();
            return contratos;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Contrato>> Post([FromServices] DataContext context, [FromBody] Contrato contrato)
        {
            double valorPrestacao = (double)contrato.ValorFinanciado/contrato.QuantidadeParcelas;
            var dataVencimento = DateTime.Today.Date.AddDays(30);
            var dataPagamento = DateTime.Today.Date.AddDays(25);
            var status = "Baixada";
            if(ModelState.IsValid)
            {
                context.Contratos.Add(contrato);
                for(var i = 0; i<contrato.QuantidadeParcelas; i++)
                {
                    Prestacao prestacao = new Prestacao();
                    prestacao.ContratoId = contrato.Id;
                    prestacao.DataVencimento = dataVencimento;
                    prestacao.DataPagamento = dataPagamento;
                    prestacao.Valor = valorPrestacao;
                     if(prestacao.DataVencimento >= DateTime.Today.Date && prestacao.DataPagamento.Equals(null))
                        status = "Aberta";
                    else if(prestacao.DataVencimento < DateTime.Today.Date && prestacao.DataPagamento.Equals(null))
                        status = "Atrasada";
                    prestacao.Status = status;
                    context.Prestacoes.Add(prestacao);
                    dataVencimento = dataVencimento.AddDays(30);
                    dataPagamento = dataPagamento.AddDays(25);
                }
                await context.SaveChangesAsync();
                return contrato;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<Contrato>> DeleteContrato(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);

            if(contrato == null)
            {
                return NotFound();
            }

            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            return contrato;
        }

    }
}