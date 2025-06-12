using APIEstudiantes.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEstudiantes
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
           : base(options)
        {
        }

        // Colocar todos los modelos
        public DbSet<Curso> Curso {  get; set; }
        public DbSet<Estudiante> Estudiante {  get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
