using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto.v1
{
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }

        public string Errors { get; set; }

        public Ent_Usuario eUsuario { get; set; }
    }
}
