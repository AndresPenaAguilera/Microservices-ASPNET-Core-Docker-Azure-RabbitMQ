using System;

namespace TiendaServicios.RabbitMQ.Bus
{
    public abstract class Comando: Message
    {
        public DateTime Timestamp { get; protected set; }

        public Comando() 
        {
            Timestamp = DateTime.Now;
        }
    }
}