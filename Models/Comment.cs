using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Models;

namespace TB_Social_Media.Models
{
    public class Comment
    {
        public int Id { get; set; } 
        public string Content { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        public User User { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}

 