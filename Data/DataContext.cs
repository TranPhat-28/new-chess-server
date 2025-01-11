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
        public DbSet<PracticeModeGameHistory> PracticeModeGameHistories => Set<PracticeModeGameHistory>();
        public DbSet<MoveHistoryItem> MoveHistoryItems => Set<MoveHistoryItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UsersFriends",  // Name of the join table
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),  // Foreign key for the first user
                    j => j.HasOne<User>().WithMany().HasForeignKey("FriendId")  // Foreign key for the second user
                );
        }
    }
}