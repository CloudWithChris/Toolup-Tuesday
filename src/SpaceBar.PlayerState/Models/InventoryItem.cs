namespace SpaceBar.PlayerState.Models
{
    public class InventoryItem
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public InventoryItemCategory? Category { get; set; }

        public int Value { get; set; }
    }
}