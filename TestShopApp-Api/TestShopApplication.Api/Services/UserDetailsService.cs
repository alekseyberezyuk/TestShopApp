using System;
using TestShopApplication.Dal.Models;
using TestShopApplication.Dal.Repositories;
using TestShopApplication.Api.Models;

namespace TestShopApplication.Api.Services
{
    public class UserDetailsService
    {
        private IUserDetailsRepository UserDetailsRepository { get; }

        public UserDetailsService(IUserDetailsRepository userDetailsRepository)
        {
            UserDetailsRepository = userDetailsRepository;
        }

        public UserDetailsRepresentation GetUserDetails(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                UserDetails dbUserDetails = UserDetailsRepository.GetUserDetails(userId);
                return new UserDetailsRepresentation(dbUserDetails);
            }
            return null;
        }

        internal bool UpdateUserDetails(UserDetailsRepresentation userDetailsRepresentation)
        {
            return UserDetailsRepository.UpdateUserDetails(userDetailsRepresentation);
        }
    }
}
