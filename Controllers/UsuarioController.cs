using chatApi.Data;
using chatApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace chatApi.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get([FromBody] ValidarUsuarioModel usuario)
        {
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
