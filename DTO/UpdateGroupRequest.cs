using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TB_Social_Media.DTO
{
    public class UpdateGroupRequest
    {
        public string Name { get; set; }
        public List<int> AdminUserIds { get; set; }
        public List<int> MemberUserIds { get; set; }
    }
}