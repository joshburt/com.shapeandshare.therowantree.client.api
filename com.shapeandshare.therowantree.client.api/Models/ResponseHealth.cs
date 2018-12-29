using System;
using System.Collections;
//using System.Linq;
//using System.Threading;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public class ResponseHealth
    {
        public bool allHealthy;
        public IDictionary databaseStatus;
        // public readonly User dataSample;

        private TrtDbContext _context;
        // private readonly IConfiguration _configuration;

        public struct DBDetails
        {
            public bool healthy;
            public string message;
        }

        public ResponseHealth(TrtDbContext context, IConfiguration config)
        {
            _context = context;
            // _configuration = config;

            allHealthy = false;

            DBDetails newDefaultConStatusObj = new DBDetails
            {
                healthy = false,
                message = "Connection attempt not yet made."
            };
            databaseStatus = new Hashtable
            {
                ["DefaultConnection"] = newDefaultConStatusObj
            };

            UpdateDbContextStatus(DbContextStatus());
            // await Task.run(DbContextStatus());
            // dataSample = _context.User.FirstOrDefault();
        }

        private async Task<bool> DbContextStatus() {
            bool dBContextStatus = await _context.Database.CanConnectAsync();
            allHealthy = dBContextStatus;

            DBDetails defaultConStatusObj = (DBDetails)databaseStatus["DefaultConnection"];
            defaultConStatusObj.healthy = dBContextStatus;
            defaultConStatusObj.message = "";
            databaseStatus["DefaultConnection"] = defaultConStatusObj;

            return dBContextStatus;
        }

        private void UpdateDbContextStatus(Task t) {
            try
            {
                t.Wait();
            }
            catch (Exception e)
            {
                DBDetails defaultConStatusObj = (DBDetails)databaseStatus["DefaultConnection"];
                defaultConStatusObj.healthy = false;
                defaultConStatusObj.message = e.Message.ToString();
                databaseStatus["DefaultConnection"] = defaultConStatusObj;
            }
        }
    }
}
