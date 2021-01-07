using System.Collections.Generic;
using System.Threading.Tasks;
using ContratosAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContratosAPI.Services
{
    public interface IContratoService
    {
        Task<Contrato> GetContratoService(int id);
        Task<List<Contrato>> GetContratosService();
        Task<ActionResult<Contrato>> PostContratoService([FromBody] Contrato contrato);
        Task<ActionResult<Contrato>> PutContratoService(int id, [FromBody] Contrato contrato);
        Task<ActionResult<Contrato>> DeleteContratoService(int id);
    }
}