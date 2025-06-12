# API de Estudiantes

# Descripción
API REST desarrollada en C# con ASP.NET Core para gestionar estudiantes y cursos. Permite realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre estudiantes y cursos, con soporte para relaciones uno a muchos entre cursos y estudiantes. Utiliza Entity Framework Core para interactuar con una base de datos SQL Server.

# Características
- Endpoints CRUD: Gestión completa de estudiantes y cursos (GET, POST, PUT, DELETE).
- Relaciones: Soporte para relaciones uno a muchos entre cursos y estudiantes.
- Base de datos: Conexión a SQL Server mediante Entity Framework Core.


# Uso
# Endpoints de Estudiantes
- GET /api/estudiantes: Obtiene la lista de todos los estudiantes.
- GET /api/estudiantes/{id}: Obtiene un estudiante por su ID.
- POST /api/estudiantes: Crea un nuevo estudiante (enviar JSON con Nombre, Calificacion y opcionalmente CursoId).
- PUT /api/estudiantes/{id}: Actualiza un estudiante existente.
- DELETE /api/estudiantes/{id}: Elimina un estudiante.
  
# Ejemplo de uso POST/estudiantes
{
  "nombre": "Juan Pérez",
  "calificacion": 85.5,
  "cursoId": 1
}

# Endpoints de Cursos
- GET /api/cursos: Obtiene la lista de todos los cursos con sus estudiantes asociados.
- GET /api/cursos/{id}: Obtiene un curso por su ID, incluyendo sus estudiantes.
- POST /api/cursos: Crea un nuevo curso (enviar JSON con Nombre).
- PUT /api/cursos/{id}: Actualiza un curso existente.
- DELETE /api/cursos/{id}: Elimina un curso.

# Ejemplo de uso POST/Cursos
{
  "nombre": "Matemáticas"
}
