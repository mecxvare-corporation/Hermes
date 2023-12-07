namespace UserService.Domain.Entities
{
    public class Interest : Entity
    {
        public string Name { get; private set; }

        private Interest()
        {

        }

        public Interest(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public List<User> Users { get; private set; } = new List<User>();//hmm moica
    }
}