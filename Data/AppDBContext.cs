using Microsoft.EntityFrameworkCore;
using cromosmundial_proyecto_final.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cromosmundial_proyecto_final.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(
            DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Jugador> Jugadores { get; set; }

        public DbSet<Seleccion> Selecciones { get; set; }

        public DbSet<Mundial> Mundiales { get; set; }

        public DbSet<Cromo> Cromos { get; set; }
    }
}