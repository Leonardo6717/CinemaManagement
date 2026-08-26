namespace CinemaManagement.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int SessaoId { get; set; }

        public int AssentoId { get; set; }

        public DateTime DataReserva { get; set; } = DateTime.Now;

        public DateTime ExpiraEm { get; set; }

        public string Status { get; set; } = "Pendente";

        // Relacionamentos
        public Cliente Cliente { get; set; } = null!;

        public Sessao Sessao { get; set; } = null!;

        public Assento Assento { get; set; } = null!;
    }
}
