

using APIEstudiantes.Models;
using APIEstudiantes.Repository.IRepository;
using APIEstudiantes.Services.IServices;

namespace APIEstudiantes.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IEstudianteRepository _repository;

        public EstudianteService(IEstudianteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Estudiante> AddValidationAsync(Estudiante estudiante)
        {
            if(estudiante.Calificacion < 0 || estudiante.Calificacion > 10)
                throw new ArgumentException("La calificación debe estar en el siguiente rango [0-10]");

            return await _repository.AddAsync(estudiante);
               
        }
    }
}
