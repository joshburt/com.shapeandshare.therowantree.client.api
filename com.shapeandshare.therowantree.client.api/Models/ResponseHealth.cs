using System;
using System.Collections;
using System.Configuration;
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

            string conString = Microsoft
            .Extensions
            .Configuration
            .ConfigurationExtensions
            .GetConnectionString(_configuration, "DefaultConnection");

            databaseStatus["database"] = conString;
        }
    }
}
