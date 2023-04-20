using System;
using System.ComponentModel.DataAnnotations;

namespace TestShopApplication.Shared.Dtos
{
    public sealed class UserDetailsRepresentation
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public UserDetailsRepresentation()
        {
        }
    }
}
