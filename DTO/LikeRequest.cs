using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TB_Social_Media.DTO
{
    public class LikeRequest
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}