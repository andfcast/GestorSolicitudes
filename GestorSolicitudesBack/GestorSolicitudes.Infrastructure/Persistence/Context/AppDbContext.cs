using GestorSolicitudes.Domain.Entities;
using GestorSolicitudes.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Solicitud> Solicitudes => Set<Solicitud>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configuración de la entidad Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn(1, 1);
                entity.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(15);
                entity.Property(u => u.NombreCompleto).HasMaxLength(60).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Rol)
                            .HasConversion<string>()
                                .HasMaxLength(20);
                entity.Property(u => u.FechaCreacion).IsRequired();
            });

            // 2. Configuración de la entidad Solicitud
            modelBuilder.Entity<Solicitud>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn(1, 1);

                // Código único de la solicitud
                entity.HasIndex(s => s.Codigo).IsUnique();
                entity.Property(s => s.Codigo).IsRequired().HasMaxLength(20);

                entity.Property(s => s.Titulo).IsRequired().HasMaxLength(150); // Obligatorio[cite: 1]
                entity.Property(s => s.Descripcion).IsRequired().HasMaxLength(2000); // Obligatorio[cite: 1]
                entity.Property(s => s.Cliente).IsRequired().HasMaxLength(100); // Obligatorio[cite: 1]

                // Conversión de Enums a String en la Base de Datos
                entity.Property(s => s.Prioridad)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(s => s.Estado)
                      .HasConversion<string>()
                      .HasMaxLength(20);
                entity.Property(s => s.FechaCreacion)
                        .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(s => s.FechaCierre)
                      .IsRequired(false);

                entity.HasOne(s => s.UsuarioResponsable)
                      .WithMany()
                      .HasForeignKey(s => s.UsuarioResponsableId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // 3. Datos de prueba (Seed Data)
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    NombreCompleto = "Administrador del Sistema",
                    NombreUsuario = "admin",
                    Email = "admin@empresa.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Rol = RolUsuario.Administrador,
                    FechaCreacion = DateTime.UtcNow
                },
                new Usuario
                {
                    Id = 2,
                    NombreCompleto = "Agente de Soporte 1",
                    NombreUsuario = "agente1",
                    Email = "agente1@empresa.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Agente123!"),
                    Rol = RolUsuario.Agente, 
                    FechaCreacion = DateTime.UtcNow
                }
            );
        }
    }
}
