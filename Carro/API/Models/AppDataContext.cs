using System;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class AppDataContext : DbContext
{
    public DbSet<Carro> Carros { get; set; }
    public DbSet<Modelo> Modelos { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=20Comer.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Modelo>().HasData(
           new Modelo() {Id = 1, Name = "Hatch"},
           new Modelo() {Id = 2, Name = "Suv"},
            new Modelo() {Id = 3, Name = "Sedam"}
        );
    }

}