﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto.v1
{
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
