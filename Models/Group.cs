using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Models;

namespace TB_Social_Media.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}