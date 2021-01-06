using System.ComponentModel.DataAnnotations;

namespace ContratosAPI.Models
{
    public class Contrato
    { 
        [Key]
        public int Id {get; set;}

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public DataType DataContratacao {get; set;}

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int ValorFinanciado { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int Prestacoes { get; set; }
    }
}