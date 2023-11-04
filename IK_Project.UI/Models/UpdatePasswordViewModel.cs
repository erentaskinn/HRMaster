﻿using System.ComponentModel.DataAnnotations;

namespace IK_Project.UI.Models
{
    public class UpdatePasswordViewModel
    {
        [Required(ErrorMessage = "New Password is Required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W).{8,}$", ErrorMessage = "Your password must contain at least one uppercase letter, one special character, one digit, and be at least 8 characters long.")]

        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and password confirmation do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
