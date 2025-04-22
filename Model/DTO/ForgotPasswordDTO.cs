using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class ForgotPasswordDTO
    {
        public string Email { get; set; }  // Thuộc tính Email để lưu địa chỉ email người dùng
        public string NewPassword { get; set; }  // Thuộc tính NewPassword để lưu mật khẩu mới
    }

}
