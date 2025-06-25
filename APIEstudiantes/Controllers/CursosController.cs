using APIEstudiantes.Models;
using APIEstudiantes.Repository;
using APIEstudiantes.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using System.Linq;

namespace APIEstudiantes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : Controller
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly ApplicationDBContext _db;

        public CursosController(ICursoRepository cursoRepository, ApplicationDBContext db)
        {
            _cursoRepository = cursoRepository;
            _db = db;
        }

        // Endpoints

        // POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostCurso([FromBody] Curso curso)
        {
            if(curso == null || string.IsNullOrEmpty(curso.NombreCurso))
            {
                return BadRequest("Datos Inválidos");
            }
            await _cursoRepository.AddAsync(curso);
            return Ok(curso);
        }

        // GET (ALL)
        [HttpGet]
        public async Task<ActionResult<List<Curso>>> GetCursos()
        {
            var cursos = await _cursoRepository.GetAllAsync();
            if (cursos.Count == 0)
            {
                return BadRequest("No hay registros en la bd");
            }
            return Ok(cursos);
        }

        // GET (ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var cursoEncontrado = await _cursoRepository.GetByIdAsync(id);
            if (cursoEncontrado == null)
            {
                return NotFound();
            }
            return Ok(cursoEncontrado);
        }

        // GET (para extraer a todos los alumnos asignados a un curos)
        [HttpGet("reporte")]
        public async Task<IActionResult> ReporteCursos()
        {
            var reporte = await _db.Curso
                .Include(c => c.Estudiante)
                .Select(c => new
                {
                    CursoId = c.Id,
                    NombreCurso = c.NombreCurso,
                    TotalEstudiantes = c.Estudiante.Count
                })
                .ToListAsync();

            return Ok(reporte);
        }

        // PUT
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCurso(int id, Curso curso)
        {
            var cursoEncontrado = await _cursoRepository.GetByIdAsync(id);
            if (cursoEncontrado == null) return NotFound();

            //Actualizar campos
            cursoEncontrado.NombreCurso = curso.NombreCurso;
            await _cursoRepository.UpdateAsync(cursoEncontrado);
            return Ok(cursoEncontrado);
        }


        // DELETE
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCurso(int id)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            if (curso == null) return NotFound();
            await _cursoRepository.DeleteAsync(id);
            return Ok(curso);
        }

        // clase DTO para generar el reporte
        public class ReporteCursoDto
        {
            public string NombreCurso { get; set; } = null!;
            public int CantidadAlumnos { get; set; }
        }
    }
}
