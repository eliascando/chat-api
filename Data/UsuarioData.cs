using chatApi.Conexion;
using chatApi.Model;
using Org.BouncyCastle.Crypto.Digests;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace chatApi.Data
{
    public class UsuarioData
    {
        ConexionDB cn = new ConexionDB();

        public async Task<bool> RegistrarUsuario(UsuarioModel usuario)
        {
            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("RegistrarUsuario", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    cmd.Parameters.AddWithValue("@nombre", usuario.Usuario);
                    cmd.Parameters.AddWithValue("@imagen", usuario.Imagen);
                    cmd.Parameters.AddWithValue("@password", Encrypt(usuario.Password));

                    SqlParameter registroExitoso = new SqlParameter("@Exito", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(registroExitoso);

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return (bool)registroExitoso.Value;
                }
            }
        }
        public async Task<string>ValidarUsuario(ValidarUsuarioModel usuario)
        {
            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("ValidarUsuario", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    cmd.Parameters.AddWithValue("@password", Encrypt(usuario.Password));

                    SqlParameter usuarioNombre = new SqlParameter("@User", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(usuarioNombre);

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return (string)usuarioNombre.Value;
                }
            }
        }
        public async Task<List<ListarUsuarioModel>>ListarUsuarios(string id)
        {
            var lista = new List<ListarUsuarioModel>();
            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("ListarUsuarios", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    await sql.OpenAsync();
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var usuario = new ListarUsuarioModel();
                            usuario.Id = (string)item["id"];
                            usuario.Usuario = (string)item["nombre"];
                            usuario.Imagen = (string)item["imagen"];
                            lista.Add(usuario);
                        }
                    }
                }
            }
            return lista;
        }
        public async Task<List<ListarAmigosModel>> ListarAmigos(string id)
        {
            var lista = new List<ListarAmigosModel>();
            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("ListarAmigos", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    await sql.OpenAsync();
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var amigo = new ListarAmigosModel();
                            amigo.Id = (string)item["IdAmigo"];
                            amigo.Usuario = (string)item["NombreAmigo"];
                            amigo.Imagen = (string)item["ImagenAmigo"];
                            amigo.IdRoom = (int)item["idRoom"];
                            lista.Add(amigo);
                        }
                    }
                }
            }
            return lista;
        }
        public async Task EnviarSolicitud(string idSender, string idRequested)
        {
            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("EnviarSolicitud", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUserSend", idSender);
                    cmd.Parameters.AddWithValue("@idUserReq",idRequested);

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task AtenderSolicitud(string idResponse, string idReq, string response)
        {
            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("AtenderSolicitud", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuarioResponse", idResponse);
                    cmd.Parameters.AddWithValue("@IdUsuarioReq", idReq);
                    cmd.Parameters.AddWithValue("@Respuesta", response);

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<List<ListarUsuarioModel>> ObtenerNotificaciones(string id)
        {
            var lista = new List<ListarUsuarioModel>();
            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("ObtenerNotificaciones", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    await sql.OpenAsync();
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var usuario = new ListarUsuarioModel();
                            usuario.Id = (string)item["idUsuarioSend"];
                            usuario.Usuario = (string)item["Nombre"];
                            usuario.Imagen = (string)item["Imagen"];
                            lista.Add(usuario);
                        }
                    }
                }
            }
            return lista;
        }
        public static string Encrypt(string entrada)
        {
            Sha3Digest sha3 = new Sha3Digest(256);
            byte[] inputBytes = Encoding.UTF8.GetBytes(entrada);
            byte[] hash = new byte[sha3.GetDigestSize()];
            sha3.BlockUpdate(inputBytes, 0, inputBytes.Length);
            sha3.DoFinal(hash, 0);
            return Convert.ToBase64String(hash);
        }
    }
}