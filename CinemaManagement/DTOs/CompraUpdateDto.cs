namespace CinemaManagement.DTOs
{
    public class CompraUpdateDto
    {
        public decimal ValorTotal { get; set; }

        public string FormaPagamento { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public bool Ativa { get; set; }
    }
}