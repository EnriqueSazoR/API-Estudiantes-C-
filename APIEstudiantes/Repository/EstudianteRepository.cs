using APIEstudiantes.Models;
using APIEstudiantes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIEstudiantes.Repository
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly ApplicationDBContext _db;

        public EstudianteRepository(ApplicationDBContext db)
        {
            _db = db;
            
        }
        public async Task<Estudiante> AddAsync(Estudiante estudiante)
        {
            await _db.Estudiante.AddAsync(estudiante);
            await _db.SaveChangesAsync();
            return estudiante;

        }

        public async Task DeleteAsync(int id)
        {
            var estudiante = await GetByIdAsync(id);
            if(estudiante != null)
            {
                _db.Estudiante.Remove(estudiante);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Estudiante>> GetAllAsync()
        {
            return await _db.Estudiante.ToListAsync();
        }

        public async Task<Estudiante> GetByIdAsync(int id)
        {
            return await _db.Estudiante.FindAsync(id);
        }

        public async Task UpdateAsync(Estudiante estudiante)
        {
            _db.Estudiante.Update(estudiante);
            await _db.SaveChangesAsync();
        }
    }
}
