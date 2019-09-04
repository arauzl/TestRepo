using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebTicket.Models
{
    public partial class ticketDBContext : DbContext
    {
        public ticketDBContext()
        {
        }

        public ticketDBContext(DbContextOptions<ticketDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Colas> Colas { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=192.168.1.60;Database=ticketDB;User=sa;Password=d1d1l0c0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Colas>(entity =>
            {
                entity.ToTable("colas");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cola).HasColumnName("cola");

                entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("ticket");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
