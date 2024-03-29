﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models
{
    public class SignInInput
    {
        [Required]
        [Display(Name ="e-Mail adresiniz")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }
       
        [Display(Name = "Beni Hatırla")]
        public bool IsRemember { get; set; }
    }
}
