using System;
using PasswordSecurity;

namespace ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {

            
            Console.WriteLine(
                PasswordStorage.CreateHash("jacky-home1990")
            );
        }
    }
}
