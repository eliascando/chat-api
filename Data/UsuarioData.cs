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
