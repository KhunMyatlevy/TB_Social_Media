using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Models;

namespace TB_Social_Media.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Post Post { get; set; } 
    }
}