using APIEstudiantes.Models;

namespace APIEstudiantes.Services.IServices
{
    public interface IEstudianteService
    {
        Task<Estudiante> AddValidationAsync(Estudiante estudiante);
    }
}
