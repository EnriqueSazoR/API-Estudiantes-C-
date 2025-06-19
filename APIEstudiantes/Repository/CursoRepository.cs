using APIEstudiantes.Models;
using APIEstudiantes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIEstudiantes.Repository
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ApplicationDBContext _db;

        public CursoRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Curso curso)
        {
            await _db.Curso.AddAsync(curso);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var curso = await GetByIdAsync(id);
            if (curso != null)
            {
                _db.Curso.Remove(curso);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Curso>> GetAllAsync()
        {
            return await _db.Curso.ToListAsync();
        }

        public async Task<Curso> GetByIdAsync(int id)
        {
            return await _db.Curso.FindAsync(id);
        }

        public async Task UpdateAsync(Curso curso)
        {
            _db.Curso.Update(curso);
            await _db.SaveChangesAsync();
        }
    }
}
