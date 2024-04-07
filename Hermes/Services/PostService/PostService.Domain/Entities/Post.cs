namespace PostService.Domain.Entities
{
    public class Post : Entity
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public Guid UserId { get; private set; }
        public string? Image { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Post()
        {
            
        }

        public Post(Guid userId, string title, string content, string? image = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Title = title;
            Content = content;
            Image = image;
            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public void Update(string title, string content, string? image=null)
        {
            if (image!=null)
            {
                Image = image;
            }

            Title = title;
            Content = content;
            UpdatedAt = DateTime.Now;
        }
    }
}
