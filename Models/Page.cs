using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Models;

namespace TB_Social_Media.Models
{
  public class Page
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int OwnerId { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Owner { get; set; } 
}
}