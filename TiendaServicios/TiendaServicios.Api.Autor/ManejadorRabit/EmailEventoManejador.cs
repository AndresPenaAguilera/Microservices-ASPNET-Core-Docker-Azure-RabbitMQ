using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;
using TiendaServicios.RabbitMQ.Bus;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {

        private readonly ILogger<EmailEventoManejador> _logger;
        private readonly ISendGridEnviar _sendGridEnviar;
        private readonly IConfiguration _configuration;

        public EmailEventoManejador(ILogger<EmailEventoManejador> logger, ISendGridEnviar sendGridEnviar, IConfiguration configuration)
        {
            _logger = logger;
            _sendGridEnviar = sendGridEnviar;
            _configuration = configuration;
        }

        public async Task Handle(EmailEventoQueue @event)
        {
            _logger.LogInformation($"El evento es { @event.Titulo }");

            var objData = new SendGridData();
            objData.Contenido = @event.Contenido;
            objData.emailDestinatario = @event.Destinatario;
            objData.NombreDestinatario = @event.Destinatario;
            objData.Titulo = @event.Titulo;
            objData.SendGridAPIKey = _configuration["SendGrid:ApiKey"];


            var resultado = await _sendGridEnviar.EnviarEmail(objData);

            if (resultado.resultado) 
            {
                await Task.CompletedTask;
                return;
            }
        }
    }
}
