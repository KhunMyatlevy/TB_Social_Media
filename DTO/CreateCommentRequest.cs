using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TB_Social_Media.DTO
{
    public class CreateCommentRequest
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}