using System;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class AppDataContext : DbContext
{
    public DbSet<Carro> Carros {get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=20Comer.db");
    }



}