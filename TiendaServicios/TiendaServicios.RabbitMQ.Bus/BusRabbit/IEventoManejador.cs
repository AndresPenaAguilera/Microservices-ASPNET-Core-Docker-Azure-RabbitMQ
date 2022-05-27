using System.Threading.Tasks;

namespace TiendaServicios.RabbitMQ.Bus
{
    public interface IEventoManejador<in TEvent> : IEventoManejador where TEvent: Evento
    {
        Task Handle(TEvent @event);
    }

    public interface IEventoManejador { 
    
    }
}