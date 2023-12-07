namespace UserService.Domain.Entities
{
    public class User : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public List<Interest> Interests { get; set; } = new List<Interest>(); //es mravlobitshi erqva adrec?ki

        private User()
        {

        }

        public User(string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public void Update(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public void AddInterest(Interest interest)
        {
            if (Interests is null)
            {
                Interests = new List<Interest>();
            }
            else if (!Interests.Any(x => x == interest))
            {
                Interests.Add(new Interest(interest.Name));
            }
        }
    }
}
