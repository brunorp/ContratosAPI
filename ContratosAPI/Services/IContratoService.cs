using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ContratosAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContratosAPI.Services
{
    public interface IContratoService
    {
        Task<ActionResult<Contrato>> GetContratoService(int id);
        Task<ActionResult<List<Contrato>>> GetContratosService();
        Task<ActionResult<Contrato>> PostContratoService([FromBody] Contrato contrato);
        Task<ActionResult<Contrato>> PutContratoService(int id, [FromBody] Contrato contrato);
        Task<ActionResult<Contrato>> DeleteContratoService(int id);
        void PostPrestacao(Contrato contrato, int id);
    }
}