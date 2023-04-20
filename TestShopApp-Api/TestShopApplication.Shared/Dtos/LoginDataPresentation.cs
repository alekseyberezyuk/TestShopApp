using System;
using System.ComponentModel.DataAnnotations;

namespace TestShopApplication.Shared.Dtos
{
    public sealed class LoginDataPresentation
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
