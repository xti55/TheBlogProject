﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogProject.ViewModels
{
    public class ContactMe
    {
        [Required]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} but no more than {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} but no more than {1} characters long.", MinimumLength = 2)]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} but no more than {1} characters long.", MinimumLength = 2)]
        public string Subject { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} but no more than {1} characters long.", MinimumLength = 2)]
        public string Message { get; set; }
    }
}
