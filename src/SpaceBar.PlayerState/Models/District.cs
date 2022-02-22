namespace SpaceBar.PlayerState.Models
{
    public class District
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public int Population { get; set; }

        /// <summary>
        /// 1-10 scale, 1 poorest, 10 richest
        /// </summary>
        public int EcomonicRating { get; set; }
    }
}