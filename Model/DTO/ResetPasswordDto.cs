﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string OTP { get; set; }
    }
}
