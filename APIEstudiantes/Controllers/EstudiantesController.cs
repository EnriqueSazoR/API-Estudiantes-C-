using APIEstudiantes.Models;
using APIEstudiantes.Repository.IRepository;
using APIEstudiantes.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIEstudiantes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : Controller
    {
        private readonly IEstudianteRepository _estudianteRepository;
        private readonly IEstudianteService _estudianteService;

        public EstudiantesController(IEstudianteRepository estudianteRepository, 
            IEstudianteService estudianteService)
        {
            _estudianteRepository = estudianteRepository;
            _estudianteService = estudianteService;
        }

        // Endpoints

        //POST
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante([FromBody] Estudiante estudiante)
        {
            try
            {
                var nuevoEstudiante = await _estudianteService.AddValidationAsync(estudiante);
                return Ok(nuevoEstudiante);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        //GETALL
        [HttpGet]
        public async Task<ActionResult<List<Estudiante>>> GetEstudiantes()
        {
            var estudiantes = await _estudianteRepository.GetAllAsync();
            if (estudiantes == null)
            {
                return BadRequest("No hay datos");
            }
            return Ok(estudiantes);

        }

        //GET(ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudianteBuscado = await _estudianteRepository.GetByIdAsync(id);
            if (estudianteBuscado == null)
            {
                return NotFound();
            }
            return Ok(estudianteBuscado);
        }

        //PUT(ID)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            var estudianteEncontrado = await _estudianteRepository.GetByIdAsync(id);
            if (estudianteEncontrado == null)
            {
                return NotFound();
            }

            // Actualizar campos
            estudianteEncontrado.Nombre = estudiante.Nombre;
            estudianteEncontrado.Calificacion = estudiante.Calificacion;
            await _estudianteRepository.UpdateAsync(estudianteEncontrado);
            return Ok(estudianteEncontrado);

        }

        //DELETE(ID)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _estudianteRepository.GetByIdAsync(id);
            if (estudiante == null) return NotFound();
            await _estudianteRepository.DeleteAsync(id);
            return Ok(estudiante);

        }
    }
}
