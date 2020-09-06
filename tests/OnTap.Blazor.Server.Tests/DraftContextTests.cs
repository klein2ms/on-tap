using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using OnTap.Blazor.Server.Data;
using Xunit;
using OnTap.Blazor.Server.Models;
using System.Collections.Generic;

namespace OnTap.Blazor.Server.Tests
{
    public class DraftContextTests
    {        
        [Fact]
        public async Task SeedAsync_WhenDatabaseHasNoData_WritesToDatabase()
        {
            using var conn = new SqliteConnection("Datasource=:memory:");
            try
            {
                conn.Open();

                var options = new DbContextOptionsBuilder<DraftContext>()
                    .UseSqlite(conn)
                    .Options;

                using (var context = new DraftContext(options))
                {
                    var players = new List<Player>
                    {
                        new Player { Name = "Tom Brady", Position = Shared.Position.Quarterback },
                        new Player { Name = "Antionio Brown", Position = Shared.Position.WideReceiver },
                    };
                    var playersAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(players);
                    _ = await context.SeedAsync(playersAsJson);
                }

                using (var context = new DraftContext(options))
                {
                    var actual = await context.Players.CountAsync();
                    actual.Should().Be(2);
                }                
            }
            finally
            {
                conn.Close();
            }            
        }
    }
}
