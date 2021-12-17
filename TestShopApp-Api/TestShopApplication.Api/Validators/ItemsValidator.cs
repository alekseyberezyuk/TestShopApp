using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestShopApplication.Api.Validators
{    
    public sealed class ItemsValidator : IItemsValidator
    {
        private static readonly Regex _searchParamRegex = new("^[ 0-9a-zA-Z,.\\-='!@#$%^&*():\\/\\\\]*$", RegexOptions.Compiled);

        public (bool isSuccess, string error) Validate(
            decimal minPrice = 0,
            decimal? maxPrice = null,
            string categories = null,
            string searchParam = null,
            int? pageNumber = null,
            int? itemsPerPage = null)
        {
            if (minPrice < 0 || maxPrice < 0)
            {
                return (false, "Both max price and min price must be greater than 0");
            }
            if (minPrice >= maxPrice)
            {
                return (false, "Max price must be greater than min price");
            }
            if (searchParam != null)
            {
                if (searchParam.Length > 1 && string.IsNullOrWhiteSpace(searchParam))
                {
                    return (false, "Search parameter cannot consist of whitespace characters");
                }
                if (searchParam.Length > 30)
                {
                    return (false, "Search parameter max length is 30 characters");
                }
                if (!_searchParamRegex.IsMatch(searchParam))
                {
                    return (false, "Invalid search parameter");
                }
            }
            if (pageNumber <= 0)
            {
                return (false, "Page number must be greater than 0");
            }
            if (itemsPerPage < 0)
            {
                return (false, "Items per page must be greater than 0");
            }
            if (itemsPerPage >= 100)
            {
                return (false, "Max allowed items per page is 100");
            }
            if (categories != null)
            {
                if (string.IsNullOrWhiteSpace(categories))
                {
                    return (false, "Categories cannot be empty or consist only of whitespace chars");
                }
                if (!categories.All(c => c == ',' || char.IsDigit(c)))
                {
                    return (false, "Categories cannot contain other characters than digits and commas");
                }
                if (categories[0] == ','
                    || categories[^1] == ','
                    || categories.IndexOf(",,") > -1)
                {
                    return (false, "Invalid format for parameter 'categories'. Please use comma separated ids");
                }
            }
            return (true, null);
        }
    }
}
