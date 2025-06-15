namespace CasinoMvc.Models
{
    public class PlayerArchive
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string BetInfo { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Money { get; set; }
        public int Result { get; set; }

    }
}
