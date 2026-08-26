using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTOs
{
    public class ClienteCreateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres.")]
        [RegularExpression(
            @"^[A-Za-zÀ-ÖØ-öø-ÿ' -]+$",
            ErrorMessage = "O nome deve conter apenas letras."
        )]
        public string Nome { get; set; } = string.Empty;


        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [RegularExpression(
            @"^[0-9]{10,11}$",
            ErrorMessage = "O telefone deve conter 10 ou 11 números."
        )]
        public string Telefone { get; set; } = string.Empty;


        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }
    }
}