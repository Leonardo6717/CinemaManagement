namespace CinemaManagement.DTOs
{
    public class AssentoSessaoDto
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public bool Ocupado { get; set; }
    }
}
