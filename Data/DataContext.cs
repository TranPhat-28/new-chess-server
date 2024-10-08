using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace new_chess_server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<GameStatistic> GameStatistics => Set<GameStatistic>();
        public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();
    }
}