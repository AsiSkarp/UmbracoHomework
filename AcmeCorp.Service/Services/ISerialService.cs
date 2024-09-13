using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Service.Services
{
    public interface ISerialService
    {
        Task<bool> IsSerialNumberInDatabaseAsync(string serialNumber);
        Task OccupySerialNumberDatabase();
    }
}
