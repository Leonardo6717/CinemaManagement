using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTOs
{
    public class IngressoCreateDto
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Informe um cliente válido."
        )]
        public int ClienteId { get; set; }


        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Informe uma sessão válida."
        )]
        public int SessaoId { get; set; }


        [Required(
            ErrorMessage = "O assento é obrigatório."
        )]
        [RegularExpression(
            @"^[A-Za-z][0-9]+$",
            ErrorMessage = "Informe um código de assento válido. Exemplo: A1."
        )]
        public string Assento { get; set; } =
            string.Empty;


        [Range(
            0.01,
            9999.99,
            ErrorMessage = "O preço deve ser maior que zero."
        )]
        public decimal PrecoPago { get; set; }
    }
}