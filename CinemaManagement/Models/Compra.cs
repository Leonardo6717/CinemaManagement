namespace CinemaManagement.Models
{
    public class Compra
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public decimal ValorTotal { get; set; }

        public string FormaPagamento { get; set; } = string.Empty;

        public DateTime DataCompra { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Concluida";

        public bool Ativa { get; set; } = true;
    }
}