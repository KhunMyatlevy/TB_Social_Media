using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Models;

namespace TB_Social_Media.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Createdat { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }
    }
}