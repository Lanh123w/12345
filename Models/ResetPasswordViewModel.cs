using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace WEBDOAN.Models;
public class ResetPasswordViewModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string Token { get; set; }
}
