using AcmeCorp.Data.Models.Entities;
using AcmeCorp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                string txtPath = ".\\serial_numbers.txt";

                using (StreamReader reader = new StreamReader(txtPath))
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
