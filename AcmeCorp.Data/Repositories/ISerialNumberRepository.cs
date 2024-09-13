using AcmeCorp.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Data.Repositories
{
    public interface ISerialNumberRepository
    {
        Task<bool> IsSerialNumberInDatabaseAsync(string serialNumber);
        Task<bool> IsAnySerialNumberInDatabaseAsync();
        Task SerialNumberAddAsync(SerialNumber serialNumber);
        Task SerialNumbersSaveChangesAsync();
    }
}
