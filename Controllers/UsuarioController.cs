using chatApi.Data;
using chatApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace chatApi.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet("validar/{id}/{password}")]
        public async Task<ActionResult<string>> ValidarUsuario(string id, string password)
        {
            var usuario = new ValidarUsuarioModel
            {
                Id = id,
                Password = password
            };
            var funcion = new UsuarioData();
            return await funcion.ValidarUsuario(usuario);
        }
        [HttpPost]
        public async Task<bool> Post([FromBody] UsuarioModel usuario)
        {
            var funcion = new UsuarioData();
            return await funcion.RegistrarUsuario(usuario);
        }
    }
}
