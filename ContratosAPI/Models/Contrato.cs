using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContratosAPI.Models
{
    public class Contrato
    { 
        [Key]
        public int Id {get; private set;}

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string DataContratacao {get; set;}

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public double ValorFinanciado { get; set; }

        public List<Prestacao> Prestacoes { get; set; }
    }
}