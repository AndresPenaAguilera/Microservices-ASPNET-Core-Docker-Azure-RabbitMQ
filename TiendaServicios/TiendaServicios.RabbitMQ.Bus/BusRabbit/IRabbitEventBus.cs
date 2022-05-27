using System.Threading.Tasks;

namespace TiendaServicios.RabbitMQ.Bus.BusRabbit
{
    public interface IRabbitEventBus
    {
        Task EnviarComando<T>(T comando) where T : Comando;

        void Publish<T>(T Evento) where T : Evento;

        void Suscribe<T, TH>() where T : Evento
                                 where TH : IEventoManejador<T>;
    }
}
