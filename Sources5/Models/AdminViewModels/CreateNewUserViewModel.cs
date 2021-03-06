﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sources5.Models.AdminViewModels
{
    public class CreateNewUserViewModel
    {
        [Required]
        [Display(Name = "Email - is your user name for logging in")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

