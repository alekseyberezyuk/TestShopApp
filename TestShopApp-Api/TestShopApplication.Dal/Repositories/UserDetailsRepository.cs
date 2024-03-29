﻿using System;
using Microsoft.Data.Sqlite;
using Dapper;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    internal sealed class UserDetailsRepository : IUserDetailsRepository
    {
        private string ConnectionString { get; }

        public UserDetailsRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public UserDetails GetUserDetails(Guid userId)
        {
            var query = @"SELECT first_name, last_name, address_line_1, address_line_2, city, country, zip_code, phone_number FROM user_details UD
                         LEFT JOIN users USR
                         ON UD.[user_id]=USR.id
                         WHERE USR.id=@userId";
            using var connection = new SqliteConnection(ConnectionString);
            var userDetails = connection.QueryFirstOrDefault<UserDetails>(query, new { userId = userId.ToString() });

            if (userDetails != null)
            {
                userDetails.UserId = userId.ToString();
            }
            return userDetails;
        }

        public bool UpdateUserDetails(UserDetails userDetails)
        {
            var query1 = @"UPDATE user_details
                          SET address_line_1=@addressLine1,
                              address_line_2=@addressLine2
                              city=@city,
                              country=@country,
                              zip_code=@zipCode,
                              phone_number=@phoneNumber,
                          WHERE user_id=@userId";
            var query2 = @"UPDATE users
                           SET first_name=@firstName,
                               last_name=@lastName
                           WHERE id=@userId";
            using var connection = new SqliteConnection(ConnectionString);
            return connection.Execute(query1, userDetails) > 0
                && connection.Execute(query2, userDetails) > 0;
        }
    }
}
