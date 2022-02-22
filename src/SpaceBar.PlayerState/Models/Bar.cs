namespace SpaceBar.PlayerState.Models
{
    public class Bar
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        /// <summary>
        /// The sale value of bar
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Employees that are allocated to work in this bar.
        /// </summary>
        public Guid[]? Employees { get; set; }

        /// <summary>
        /// List of assets (and the number of them) owned by the bar (e.g. Drinks, furniture etc)
        /// </summary>
        public LedgerEntry[]? Ledger { get; set; }

        /// <summary>
        /// 0-100 ranking of bar (0 worst, 100 best)
        /// </summary>
        public int Popularity { get; set; }

        /// <summary>
        /// The part of the map the Bar is located in
        /// </summary>
        public District? District { get; set; }




    }
}