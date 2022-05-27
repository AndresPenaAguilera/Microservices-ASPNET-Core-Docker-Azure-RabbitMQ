using System;

namespace TiendaServicios.RabbitMQ.Bus
{
    public abstract class Evento 
    {
        protected Evento()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; protected set; }
    }
}