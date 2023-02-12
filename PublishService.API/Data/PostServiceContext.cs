using Microsoft.EntityFrameworkCore;

namespace PublishService.API.Data
{
    public class PostServiceContext : DbContext
    {
        public PostServiceContext(DbContextOptions<PostServiceContext> options):base(options)
        {

        }
        public DbSet<PublishService.API.Entities.Post> Post { get; set; }
        public DbSet<PublishService.API.Entities.User> User { get; set; }
    }
}
