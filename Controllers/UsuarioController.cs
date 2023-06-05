using chatApi.Data;
using chatApi.Model;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace chatApi.Controllers
{
    //Controlador de usuarios
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet("listar/{id}")]
        public async Task<ActionResult<List<ListarUsuarioModel>>> ListarUsuarios(string id)
        {
            var funcion = new UsuarioData();
            var lista = await funcion.ListarUsuarios(id);
            return Ok(lista);
        }
        [HttpGet("validar/{id}/{password}")]
        public async Task<ActionResult<ListarUsuarioModel>> ValidarUsuario(string id, string password)
        {
            var usuario = new ValidarUsuarioModel
            {
                Id = id,
                Password = password
            };
            var funcion = new UsuarioData();
            var usuarioEncontrado = await funcion.ValidarUsuario(usuario);
            if (usuarioEncontrado != null)
            {
                return Ok(usuarioEncontrado);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<bool> NuevoUsuario([FromBody] UsuarioModel usuario)
        {
            var funcion = new UsuarioData();
            return await funcion.RegistrarUsuario(usuario);
        }
        [HttpPost("solicitud/atender/{idResponse}/{idRequest}/{response}")]
        public async Task<ActionResult> AtenderSolicitud(string idResponse, string idRequest, string response)
        {
            var funcion = new UsuarioData();
            await funcion.AtenderSolicitud(idResponse, idRequest, response);

            return Ok();
        }
        [HttpPut("solicitud/{idSend}/{idReq}")]
        public async Task<ActionResult> EnviarSolicitud(string idSend, string idReq)
        {
            var funcion = new UsuarioData();
            await funcion.EnviarSolicitud(idSend, idReq);

            return Ok();
        }
    }

    // Controlador de Amigos
    [ApiController]
    [Route("api/amigos")]
    public class AmigosController : ControllerBase
    {
        [HttpGet("listar/{id}")]
        public async Task<ActionResult<List<ListarAmigosModel>>> ListarAmigos(string id)
        {
            var funcion = new UsuarioData();
            var lista = await funcion.ListarAmigos(id);
            return Ok(lista);
        }
    }

    // Controlador de Notificaciones
    [ApiController]
    [Route("api/notificaciones")]
    public class NotificacionesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ListarUsuarioModel>>> ObtenerNotificaciones(string id)
        {
            var funcion = new UsuarioData();
            var lista = await funcion.ObtenerNotificaciones(id);
            return Ok(lista);
        }
    }
    // Controlador de Notificaciones
    [ApiController]
    [Route("api/datos")]
    public class DatosController : ControllerBase
    {
        [HttpPut("imagen/{id}/{imagen}")]
        public async Task<ActionResult> GuardarImagen(string id, string imagen)
        {
            var funcion = new UsuarioData();
            string imageEncoded = imagen;
            string imageDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(imageEncoded));

            await funcion.GuardarImagen(id, imageDecoded);
            return Ok();
        }
    }
}