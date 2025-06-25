using APIEstudiantes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace APIEstudiantes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDBContext db, IConfiguration configuration)
        {   
            _db = db;
            _configuration = configuration;
        }

        //Enpoint POST
        [HttpPost("register")]
        public async Task<ActionResult> PostUsuario([FromBody] Usuario user)
        {
            if(user == null || string.IsNullOrEmpty(user.NombreUsuario) || string.IsNullOrEmpty(user.Clave) || string.IsNullOrEmpty(user.Rol))
            {
                return BadRequest(new {mensaje = "Todos los campos son obligatorios" });
            }

            // validar si el usuario existe
            if(await _db.Usuario.AnyAsync(u => u.NombreUsuario == user.NombreUsuario))
            {
                return Conflict(new { mensaje = "El nombre de usuario ya existe" });
            }
            await _db.Usuario.AddAsync(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }

        //Endponint POST
        [HttpPost("login")]
        public async Task<ActionResult> postUsuario([FromBody] UsuarioLoginDto login)
        {
           if(string.IsNullOrEmpty(login.NombreUsuario) || string.IsNullOrEmpty(login.Clave))
           {
                return BadRequest("Crendeciales Inválidas");
           }

            var usuario = await _db.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == login.NombreUsuario);
            if (usuario == null || usuario.Clave != login.Clave)
            {
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrecta" });
            }

            var token = GenerarJwtToken(usuario);
            return Ok(new { Token = token });
           
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
    }


    // DTO para la entrada del login
    public class UsuarioLoginDto
    {
        public string NombreUsuario { get; set; } = null!;
        public string Clave { get; set; } = null!;
    }
}
