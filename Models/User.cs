using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB_Social_Media.Models;

namespace HelloWorldApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}