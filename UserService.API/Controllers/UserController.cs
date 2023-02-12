using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserService.API.Data;
using UserService.API.Entities;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServiceContext _context;
        public UserController(UserServiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        private void PublishToMessagingQueue(string integrationevent, string eventData)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "user",
                routingKey: integrationevent,
                basicProperties: null,
                body: body);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id =id,
                newname = user.Name
            });

            PublishToMessagingQueue("user.update", integrationEventData);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = user.ID,
                name = user.Name
            });
            PublishToMessagingQueue("user.add", integrationEventData);
            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }


    }
}
