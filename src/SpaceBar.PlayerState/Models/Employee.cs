namespace SpaceBar.PlayerState.Models
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Species? Species { get; set; }

        public int Salary { get; set; }

        public int Age { get; set; }

    }
}