using System;
using BCryptHelper = BCrypt.Net.BCrypt;

namespace TestShopApplication.Sandbox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter a password to hash: ");
            string salt = BCryptHelper.GenerateSalt();
            string pass = Console.ReadLine();
            Console.WriteLine("\n\n");
            string passwordHash = BCryptHelper.HashPassword(pass, salt);
            Console.WriteLine($"Salt: {salt}");
            Console.WriteLine($"Hash: {passwordHash}");
            Console.WriteLine("\n\n");

            Console.Write("Please enter a password to verify: ");
            string pass2 = Console.ReadLine();

            if (BCryptHelper.Verify(pass2, passwordHash))
            {
                Console.WriteLine("The password is correct");
            }
            else
            {
                Console.WriteLine("Wrong password");
            }
            Console.ReadKey();
        }
    }
}
