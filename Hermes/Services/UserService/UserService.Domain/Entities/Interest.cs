namespace UserService.Domain.Entities
{
    public class Interest : Entity
    {
        public string Name { get; private set; }

        public List<User> Users { get; private set; } = new List<User>();

        private Interest()
        {

        }

        public Interest(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public void AddUser(User user)
        {
            if (!Users.Any(x => x.Id == user.Id))
            {
                Users.Add(user);
            }
            else
            {
                throw new InvalidOperationException("User already has the interest associated");
            }
        }
    }
}