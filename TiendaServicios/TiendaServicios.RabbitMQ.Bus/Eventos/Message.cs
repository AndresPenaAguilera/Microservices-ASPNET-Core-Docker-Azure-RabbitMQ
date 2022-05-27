using MediatR;

namespace TiendaServicios.RabbitMQ.Bus
{
    public abstract class Message : IRequest<bool>
    {
        protected Message()
        {
            MessageType = GetType().Name;
        }

        public string MessageType { get; protected set; }

    }
}