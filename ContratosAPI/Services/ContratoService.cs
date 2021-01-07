using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContratosAPI.Data;
using ContratosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContratosAPI.Services
{
    public class ContratoService : IContratoService
    {
      //  private readonly IFeatureManager _featureManager;
       // private readonly IMemoryCache _cache;
        private readonly DataContext _context;

        public ContratoService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Contrato>> GetContratoService(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            
            VerificaErro(contrato);

            contrato.Prestacoes = await _context.Prestacoes.Where(p => contrato.Id == p.ContratoId).ToListAsync();

            return contrato;
        }

        public async Task<List<Contrato>> GetContratosService()
        {
            var contratos = await _context.Contratos.ToListAsync();
            foreach(var contrato in contratos)
            {
                contrato.Prestacoes = await _context.Prestacoes.Where(p => contrato.Id == p.ContratoId).ToListAsync();
            }
            return contratos;
        }

        public async Task<ActionResult<Contrato>> PostContratoService([FromBody] Contrato contrato)
        {     
             
            contrato.Prestacoes = PostPrestacao(contrato, contrato.Id);
            _context.Contratos.Add(contrato);

            await _context.SaveChangesAsync();

            return contrato;
        }

        public async Task<ActionResult<Contrato>> PutContratoService(int id, [FromBody] Contrato contrato)
        {
            VerificaErro(contrato);

            var contratoExistente = await _context.Contratos.FindAsync(id);

            contratoExistente.DataContratacao = contrato.DataContratacao;
            contratoExistente.QuantidadeParcelas = contrato.QuantidadeParcelas;
            contratoExistente.ValorFinanciado = contrato.ValorFinanciado;

            await DeletePrestacao(id);
            PostPrestacao(contrato, id);
            await _context.SaveChangesAsync();

            contratoExistente.Prestacoes = await Task.Run(() => _context.Prestacoes.ToListAsync());

            return contratoExistente;
        }

        public async Task<ActionResult<Contrato>> DeleteContratoService(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);

            VerificaErro(contrato);

            _context.Contratos.Remove(contrato);
            await DeletePrestacao(id);
            await _context.SaveChangesAsync();

            return contrato;
        }

        private async Task DeletePrestacao(int id)
        {
            var prestacoes = await _context.Prestacoes.Where(p => id == p.ContratoId).ToArrayAsync();
            _context.Prestacoes.RemoveRange(prestacoes);
        }

        private List<Prestacao> PostPrestacao(Contrato contrato, int id)
        {   
            List<Prestacao> prestacoes = new List<Prestacao>();
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
                prestacoes.Add(prestacao);
            }            
            return prestacoes;
        }

        private async void InserePrestacoes(Contrato contrato)
        {
            var prestacoes = await Task.Run(() => _context.Prestacoes.ToListAsync()); 
            contrato.Prestacoes = prestacoes;
            await _context.SaveChangesAsync();
        }

        private void VerificaErro(Contrato contrato)
        {
            if(contrato == null)
            {
                throw new ArgumentNullException("Contrato inexistente");
            }
        }
    }
}