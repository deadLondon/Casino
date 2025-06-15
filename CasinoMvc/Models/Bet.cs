namespace CasinoMvc.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int? Number { get; set; }
        public int Money { get; set; }
        public string? Color { get; set; }
        public string? EvenOdd { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
