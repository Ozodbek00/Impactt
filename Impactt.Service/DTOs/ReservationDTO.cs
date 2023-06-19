namespace Impactt.Service.DTOs
{
    public sealed class ReservationDTO
    {
        public BookerDTO Booker { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }
    }

    public class BookerDTO
    {
        public string Name { get; set; }
    }
}
