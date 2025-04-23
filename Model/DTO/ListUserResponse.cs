using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class ListUserResponse
    {
        public string LessonTitle { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
    }
}
