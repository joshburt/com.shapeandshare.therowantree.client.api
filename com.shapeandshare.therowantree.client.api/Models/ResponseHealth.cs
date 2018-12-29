using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public class ResponseHealth
    {
        public readonly bool allHealthy;
        public readonly IDictionary databaseStatus;

        private readonly TrtDbContext _context;
        private readonly IConfiguration _configuration;

        public ResponseHealth(TrtDbContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;

            allHealthy = true;

            databaseStatus = new Hashtable();
            databaseStatus["DefaultConnection"] = "Unknown";
            UpdateDbContextStatus(DbContextStatus());
        }

        private async Task<bool> DbContextStatus() {
            return await _context.Database.CanConnectAsync();
        }

        private void UpdateDbContextStatus(Task t) {
            try
            {
                t.Wait();
                databaseStatus["DefaultConnection"] = t.ToString();
            }
            catch(Exception e)
            {
                databaseStatus["DefaultConnection"] = e.Message.ToString();
            }
        }
    }
}
