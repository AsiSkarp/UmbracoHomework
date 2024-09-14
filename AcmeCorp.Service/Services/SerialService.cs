using AcmeCorp.Data.Models.Entities;
using AcmeCorp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Service.Services
{
    public class SerialService : ISerialService
    {
        private readonly ISerialNumberRepository _serialNumberRepository;

        public SerialService(ISerialNumberRepository serialNumberRepository)
        {
            _serialNumberRepository = serialNumberRepository;
        }

        public async Task OccupySerialNumberDatabase()
        {
            var hasAnySerial = await _serialNumberRepository.IsAnySerialNumberInDatabaseAsync();
            if (!hasAnySerial)
            {
                string currentPath = Directory.GetCurrentDirectory(); // Gets the current directory
                string pathTwoLevelsUp = Path.Combine(currentPath, "..");
                string filePath = Path.Combine(pathTwoLevelsUp, "serial_numbers.txt");
                string fullPath = Path.GetFullPath(filePath);
                Debug.WriteLine("FULL PATH!!!!! ------- !!!");
                Debug.WriteLine(fullPath);
                //string txtPath = ".\\serial_numbers.txt";

                using (StreamReader reader = new StreamReader(fullPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        var sn = new SerialNumber
                        {
                            Serial = line
                        };
                        _ = _serialNumberRepository.SerialNumberAddAsync(sn);
                    }
                }
                _ = _serialNumberRepository.SerialNumbersSaveChangesAsync();
            }
        }

        public async Task<bool> IsSerialNumberInDatabaseAsync(string serialNumber)
        {
            bool hasSerial = await _serialNumberRepository.IsSerialNumberInDatabaseAsync(serialNumber);
            return hasSerial;
        }
    }
}
