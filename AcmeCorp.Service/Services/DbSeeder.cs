namespace AcmeCorp.Service.Services

{
    using System;
    using System.Linq;

    public class DbSeeder
    {
        public static string GenerateSerialNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            string GenerateSet()
            {
                return new string(Enumerable.Repeat(chars, 3)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }

            return $"ACME-{GenerateSet()}-{GenerateSet()}-{GenerateSet()}";
        }

        public static void GenerateSerialNumberFile(int numCount)
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            //string path = ".";
            string currentPath = Directory.GetCurrentDirectory(); // Gets the current directory
            string pathTwoLevelsUp = Path.Combine(currentPath, "..", "..");
            string fullPath = Path.GetFullPath(pathTwoLevelsUp);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(fullPath, "serial_numbers.txt")))
            {
                for (var i = 0; i < numCount; i++)
                {
                    outputFile.WriteLine(GenerateSerialNumber());
                }
            }

        }
    }
}
