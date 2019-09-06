using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NpgsqlEFBug
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ApplicationDbContext _dbContext;

        public Worker(ILogger<Worker> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SeedDatabase();

            while (true)
            {
                var product = await _dbContext.Products.FirstAsync();

                _logger.LogInformation("Query found: " + product.Title);
            }
        }

        private async Task SeedDatabase()
        {
            var itemCount = await _dbContext.Products.CountAsync();

            if (itemCount == 0)
            {
                _logger.LogInformation("Seeding Database");

                for (int i = 0; i < 9; i++)
                {
                    var product = new Product();
                    product.Title = "Title" + i;
                    _dbContext.Products.Add(product);
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
