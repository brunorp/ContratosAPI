using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContratosAPI.Data;
using ContratosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.FeatureManagement;

namespace ContratosAPI.Services
{
    public class ContratoService : IContratoService
    {
        private readonly IFeatureManager _featureManager;
        private readonly IMemoryCache _cache;
        private DataContext _context;
        public ContratoService(DataContext context, IMemoryCache cache, IFeatureManager featureManager)
        {
            _context = context;
             _cache = cache;
            _featureManager = featureManager;
        }

        public async Task<ActionResult<Contrato>> GetContratoService(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);

            errorMessage(contrato);

            return contrato;
        }

        public async Task<ActionResult<List<Contrato>>> GetContratosService()
        {
            var contratos = await _context.Contratos.ToListAsync();
            return contratos;
        }

        public async Task<ActionResult<Contrato>> PostContratoService([FromBody] Contrato contrato)
        {
            _context.Contratos.Add(contrato);
            PostPrestacao(contrato, contrato.Id);
            await _context.SaveChangesAsync();
            return contrato;
        }

        public async Task<ActionResult<Contrato>> PutContratoService(int id, [FromBody] Contrato contrato)
        {
            errorMessage(contrato);

            var contratoExistente = await _context.Contratos.FindAsync(id);

            contratoExistente.DataContratacao = contrato.DataContratacao;
            contratoExistente.QuantidadeParcelas = contrato.QuantidadeParcelas;
            contratoExistente.ValorFinanciado = contrato.ValorFinanciado;
            contratoExistente.Prestacoes = contrato.Prestacoes;

            var prestacoes =  await _context.Prestacoes.Where(p => id == p.ContratoId).ToArrayAsync();
            _context.Prestacoes.RemoveRange(prestacoes);
            PostPrestacao(contrato, id);

            await _context.SaveChangesAsync();

            return contratoExistente;
        }

        public async Task<ActionResult<Contrato>> DeleteContratoService(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);

            errorMessage(contrato);

            _context.Contratos.Remove(contrato);
            var prestacoes = await _context.Prestacoes.Where(p => id == p.ContratoId).ToArrayAsync();
            _context.Prestacoes.RemoveRange(prestacoes);

            await _context.SaveChangesAsync();

            return contrato;
        }

        public void PostPrestacao(Contrato contrato, int id){
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

        public void errorMessage(Contrato contrato)
        {
            if(contrato == null)
            {
                throw new System.Exception("Contrato inexistente");
            }
        }
    }
}