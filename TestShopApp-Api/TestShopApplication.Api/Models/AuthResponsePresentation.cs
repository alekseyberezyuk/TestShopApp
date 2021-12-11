namespace TestShopApplication.Api.Models
{
    public sealed class AuthResponsePresentation
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
