using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContratosAPI.Models
{
    public class Prestacao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Contrato")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int ContratoId { get; set; }
        [JsonIgnore]
        public Contrato Contrato { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public DateTime DataVencimento { get; set; }

        public DateTime? DataPagamento { get; set; } = null;

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public double Valor { get; set; }

        public string Status { get; set; }
    }
}

//Entidade Prestação: contrato, data vencimento, data pagamento, valor, status (Aberta, Baixada, Atrasada).