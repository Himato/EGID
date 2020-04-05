﻿using System.ComponentModel.DataAnnotations;

namespace EGID.Data.Cards.Dto
{
    public class SignInDto
    {
        [Required] public string PrivateKey { get; set; }

        [Required] [Range(8, 64)] public string Pin1 { get; set; }
    }
}