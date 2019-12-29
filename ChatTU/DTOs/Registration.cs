using ChatTU.DTOs.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatTU.DTOs
{
    public class Registration
    {
        [Required, MaxLength(30)]
        [NameValidation]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required, MaxLength(30)]
        [NameValidation]
        [StringLength(50, ErrorMessage = "The password must be at least 5 characters long", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password does not match the confirmation password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required, MaxLength(50)]
        [NameValidation]
        public string Firstname { get; set; }

        [Required, MaxLength(50)]
        [NameValidation]
        public string Lastname { get; set; }
    }
}