namespace CinemaManagement.DTOs
{
    public class CompraCreateDto
    {
        public int ClienteId { get; set; }

        public decimal ValorTotal { get; set; }

        public string FormaPagamento { get; set; } = string.Empty;
    }
}