﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CommunityToolShedMvc.ViewModels
{
    public class LoginViewModel
    {
        
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}