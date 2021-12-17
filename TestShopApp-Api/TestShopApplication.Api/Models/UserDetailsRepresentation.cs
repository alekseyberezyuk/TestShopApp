using System;
using System.ComponentModel.DataAnnotations;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Api.Models
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

        public UserDetailsRepresentation(UserDetails userDetails)
        {
            Id = Guid.Parse(userDetails.UserId);
            FirstName = userDetails.FirstName;
            LastName = userDetails.LastName;
            Country = userDetails.Country;
            City = userDetails.City;
            ZipCode = userDetails.ZipCode;
            PhoneNumber = userDetails.PhoneNumber;
            Address1 = userDetails.AddressLine1;
            Address2 = userDetails.AddressLine2;
        }

        public static implicit operator UserDetails(UserDetailsRepresentation obj)
        {
            return new UserDetails
            {
                UserId = obj.Id.ToString(),
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Country = obj.Country,
                City = obj.City,
                ZipCode = obj.ZipCode,
                PhoneNumber = obj.PhoneNumber,
                AddressLine1 = obj.Address1,
                AddressLine2 = obj.Address2,
            };
        }
    }
}
