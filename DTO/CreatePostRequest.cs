using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TB_Social_Media.DTO
{
    public class CreatePostRequest
    {
        public string Content { get; set; }
        public int UserId { get; set; }
    }
}