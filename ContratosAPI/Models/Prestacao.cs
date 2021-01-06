using System;
using System.ComponentModel.DataAnnotations;

namespace ContratosAPI.Models
{
    public class Prestacao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int ContratoId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public DateTime DataVencimento { get; set; }

        public DateTime? DataPagamento { get; set; } = null;

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public double Valor { get; set; }

        public string Status { get; set; }
    }
}

//Entidade Prestação: contrato, data vencimento, data pagamento, valor, status (Aberta, Baixada, Atrasada).