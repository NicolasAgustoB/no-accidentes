using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models.Common;
using PWebApi.Models.Request;
using PWebApi.Models.Response;
using PWebApi.Tools;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PWebApi.Models.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        public UserService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }
        public Usuario Auth(Usuario model)
        {
            Usuario response = new Usuario();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                var a = Encrypt.GetSHA256(model.Password).ToString();
                connection.Open();
                OracleCommand command = new OracleCommand("PCK_USUARIO.SP_VALIDAR", connection);
                if (model.UserName != null)
                {
                    command.Parameters.Add("Usuario", model.UserName);
                }
                else
                {
                    command.Parameters.Add("Usuario", model.Email);
                }              
                command.Parameters.Add("Password", Encrypt.GetSHA256(model.Password).ToString());
                
                command.Parameters.Add("id", OracleDbType.Int32, size: 10);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                model.Id = int.Parse(command.Parameters["id"].Value.ToString());
                if (model.Id > 0)
                {
                    command = new OracleCommand("select * from Usuario where ID_USUARIO = " + model.Id + "", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        response.Id = (int)reader.GetInt64(0);
                        response.Nombre = reader.GetString(1);
                        response.Apellidos = reader.GetString(2);
                        response.UserName = reader.GetString(3);
                        response.Email = reader.GetString(5);
                        response.Rut = reader.GetString(6);
                        response.Telefono = reader.GetString(7);
                        response.RolId = reader.GetInt32(8);
                        response.SucursalId = reader.GetInt32(9);
                        response.Estado = reader.GetInt32(10);
                    }
                    response.Token = GetToken(model);
                    return response;
                }
                else
                {
                    return response = null; 
                }
            }
        }
        public string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Email, usuario.Email.ToString())
                    }),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
