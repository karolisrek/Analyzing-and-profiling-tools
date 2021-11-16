using System;
using System.Security.Cryptography;
using System.Diagnostics;
using ConsoleTables;

namespace PasswordGeneration
{
    class Program
    {
        public static byte[] Salt = { 82, 122, 43, 30, 47, 97, 4, 124, 31, 63, 108, 69, 83, 86, 125, 88, 98, 77, 111, 79, 71, 73, 100, 106, 8, 20, 95, 27, 38, 32, 61, 88 };
        public static string Password = "lV7R5k9kRfSORwzEr5o1qRPe5b0eY5KFViF6CCCWgs9X4irDA4dH2nwHhN7IyX9z25SLcbh0e08LFMu0anbQKv1LF0RMBtNtBXIGmOdS2Sd0vcM5boeL8XSDqzRa";

        static void Main(string[] args)
        {
            var test = new Test();
            var result1 = RunTest(() => test.GeneratePasswordHashUsingSalt(Password, Salt));
            var result2 = RunTest(() => test.GeneratePasswordHashUsingSaltOptimized(Password, Salt));

            var table = new ConsoleTable("Name", "Return value", "Elaspsed Milliseconds")
                .AddRow("Provided", result1.ResultValue, result1.ElapsedMilliseconds)
                .AddRow("Optimized", result2.ResultValue, result2.ElapsedMilliseconds);

            table.Write();

            Console.ReadLine();
        }

        static TestResult RunTest(Func<string> testFunc)
        {
            var sw = new Stopwatch();

            sw.Start();

            var result = testFunc();

            sw.Stop();

            return new TestResult() { ResultValue = result, ElapsedMilliseconds = sw.ElapsedMilliseconds };
        }

        public class TestResult
        {
            public string ResultValue { get; set; }
            public long ElapsedMilliseconds { get; set; }
        }
    }

    public class Test
    {
        public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }

        public string GeneratePasswordHashUsingSaltOptimized(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}
