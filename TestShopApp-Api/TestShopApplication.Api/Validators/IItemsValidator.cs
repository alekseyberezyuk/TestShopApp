namespace TestShopApplication.Api.Validators
{
    public interface IItemsValidator
    {
        public (bool isSuccess, string error) Validate(
            decimal minPrice = 0,
            decimal? maxPrice = null,
            string categories = null,
            string searchParam = null,
            int? pageNumber = null,
            int? itemsPerPage = null);
    }
}
