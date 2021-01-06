using System.ComponentModel.DataAnnotations;

namespace ContratosAPI.Models
{
    public class Prestacao
    {
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int ContratoId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public DataType DataVencimento { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public DataType DataPagamento { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int Valor { get; set; }

        public string Status { get; set; }
    }
}

//Entidade Prestação: contrato, data vencimento, data pagamento, valor, status (Aberta, Baixada, Atrasada).