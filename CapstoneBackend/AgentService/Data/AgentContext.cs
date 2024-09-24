using Microsoft.EntityFrameworkCore;
using AgentMicroservice.Models;
using System.Collections.Generic;

namespace AgentMicroservice.Data
{
    public class AgentContext : DbContext
    {
        public AgentContext(DbContextOptions<AgentContext> options)
            : base(options)
        {
        }

        public DbSet<Agent> Agents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .HasKey(a => a.Id); // Define PropId as the primary key
        }
    }
}
