using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PixelCelebrateBackend.Database
{
    public partial class PixelCelebrateContext : DbContext
    {
        public PixelCelebrateContext()
        {
        }

        public PixelCelebrateContext(DbContextOptions<PixelCelebrateContext> options)
            : base(options)
        {
        }

        //Din User a facut Users;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-AV42QVU;Database=PixelCelebrate;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.DateOfJoining).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
