namespace APIEstudiantes.Test;

using APIEstudiantes.Controllers;
using APIEstudiantes.Models;
using APIEstudiantes.Repository.IRepository;
using APIEstudiantes.Services;
using APIEstudiantes.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class EstudiantesControllerTests
{
    private readonly Mock<IEstudianteRepository> _mockRepository;
    private readonly Mock<IEstudianteService> _mockService;
    private readonly EstudiantesController _controller;

    public EstudiantesControllerTests()
    {
        _mockRepository = new Mock<IEstudianteRepository>();
        _mockService = new Mock<IEstudianteService>();
        _controller = new EstudiantesController(_mockRepository.Object, _mockService.Object);
    }

    [Fact]
    public async Task GetEstudiantes_DevuelveResultadoOk_ConListaDeEstudiantes()
    {
        // Arrange
        var estudiantes = new List<Estudiante>
        {
            new Estudiante { Id = 1, Nombre = "Juan", Calificacion = 8.5 },
            new Estudiante { Id = 2, Nombre = "Ana", Calificacion = 9.0 }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(estudiantes);

        // Act
        var result = await _controller.GetEstudiantes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Estudiante>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task AddValidationAsync_CalificacionValida_DevuelveEstudiante()
    {
        // Arrange
        var estudianteEsperado = new Estudiante { Id = 1, Nombre = "Luis Sazo", Calificacion = 9.5, CursoId = 12, Curso = null };
        var mockRepository = new Mock<IEstudianteRepository>();
        var service = new EstudianteService(mockRepository.Object);
        mockRepository.Setup(repo => repo.AddAsync(It.Is<Estudiante>(e => e.Id == estudianteEsperado.Id && e.Nombre == estudianteEsperado.Nombre && e.Calificacion == estudianteEsperado.Calificacion)))
                     .ReturnsAsync(estudianteEsperado);

        // Act
        var result = await service.AddValidationAsync(estudianteEsperado);

        // Assert
        Assert.Equal(estudianteEsperado, result);
        mockRepository.Verify(repo => repo.AddAsync(It.Is<Estudiante>(e => e.Id == estudianteEsperado.Id)), Times.Once());
    }

    [Fact]
    public async Task AddValidationAsync_CalificacionMenorACero_LanzaArgumentException()
    {
        // Arrange
        var estudiante = new Estudiante { Id = 1, Nombre = "Juan", Calificacion = -1 };
        var mockRepository = new Mock<IEstudianteRepository>();
        var service = new EstudianteService(mockRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddValidationAsync(estudiante));
        Assert.Contains("La calificación debe estar en el siguiente rango [0-10]", exception.Message);
    }

    [Fact]
    public async Task AddValidationAsync_CalificacionMayorQueDiez_LanzaArgumentException()
    {
        // Arrange
        var estudiante = new Estudiante { Id = 1, Nombre = "Juan", Calificacion = 11 };
        var mockRepository = new Mock<IEstudianteRepository>();
        var service = new EstudianteService(mockRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddValidationAsync(estudiante));
        Assert.Contains("La calificación debe estar en el siguiente rango [0-10]", exception.Message);
    }
}