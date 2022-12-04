using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Controllers;
using PWebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace PWebApi.Models.HostedServices
{
    public class NotificarAtraso : IHostedService, IDisposable
    {
        private Timer _timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendEmail, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }
        public void SendEmail(object state)
        {

            var origen = "prevencionistas.grupo4@gmail.com";
            var usuarios = ReadAllAtrasados();
            var pass = "eafbcunqccfgqsrb";

            foreach (var item in usuarios)
            {
                var destino = item.Email;
                MailMessage mailMessage = new MailMessage(origen, destino, "Atraso en el pago", "<p>Aun no ha realizado el pago de su boleta</p> </br> <b>Su cuenta se inhabilitara hasta que realize el pago por el medio correspondiente</b>");
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(origen, pass);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public List<Usuario> ReadAllAtrasados()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from Usuario where estado = 2", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Usuario> result = new List<Usuario>();
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.Id = (int)reader.GetInt64(0);
                        usuario.Nombre = reader.GetString(1);
                        usuario.Apellidos = reader.GetString(2);
                        usuario.UserName = reader.GetString(3);
                        usuario.Email = reader.GetString(5);
                        usuario.Rut = reader.GetString(6);
                        usuario.Telefono = reader.GetString(7);
                        usuario.RolId = reader.GetInt32(8);
                        usuario.SucursalId = reader.GetInt32(9);
                        usuario.Estado = (int)reader.GetInt64(10);
                        result.Add(usuario);
                    }
                    return result;
                }
                catch
                {
                    List<Usuario> result = new List<Usuario>();
                    return result;
                }
            }
        }
    }
}