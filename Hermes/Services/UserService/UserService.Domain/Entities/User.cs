﻿namespace UserService.Domain.Entities
{
    public class User : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string ProfileImage { get; private set; } = string.Empty;

        public List<Interest> Interests { get; set; } = new List<Interest>();

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
            if (!Interests.Exists(x => x.Id == interest.Id))
            {
                Interests.Add(interest);
            }
            else
            {
                throw new InvalidOperationException("Interest already has been associated to current user.");
            }
        }

        public void RemoveInterest(Interest interest)
        {
            if (Interests.Exists(x => x.Id == interest.Id))
            {
                Interests.Remove(interest);
            }
            else
            {
                throw new InvalidOperationException("Interest is not associated with current user.");
            }
        }

        public void SetImageUri(string imangeName)
        {
            ProfileImage = imangeName;
        }

        public void RemoveImageUri()
        {
            ProfileImage = string.Empty;
        }
    }
}
