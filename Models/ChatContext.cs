using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chat_CodeFirst.Models
{
    public class ChatContext:DbContext
    {
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public ChatContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine)
                .UseLazyLoadingProxies()
                .UseNpgsql("Host=localhost;Username=postgres;Password=example;Database=Chat_CodeFirst_DB");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity => 
            {
                entity.HasKey(e => e.Id).HasName("message_pkey");
                entity.ToTable("Messages");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Text).HasColumnName("text");
                entity.Property(e => e.FromUserId).HasColumnName("fromUserId");
                entity.Property(e => e.ToUserId).HasColumnName("toUserId");

                entity.HasOne(d => d.FromUser).WithMany(p => p.FromMessages)
                    .HasForeignKey(e => e.FromUserId)
                        .HasConstraintName("messagesFromUserId_fkey");
                entity.HasOne(d => d.ToUser).WithMany(p => p.ToMessages)
                    .HasForeignKey(e => e.ToUserId)
                        .HasConstraintName("messagesToUserId_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("user_pkey");
                entity.ToTable("Users");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
