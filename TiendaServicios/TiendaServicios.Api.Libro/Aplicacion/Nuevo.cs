using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest 
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class Ejecutavalidacion : AbstractValidator<Ejecuta> 
        {
            public Ejecutavalidacion() 
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }   
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoLibreria _contexto;
            public readonly IRabbitEventBus _eventBus;

            public Manejador(ContextoLibreria contexto, IRabbitEventBus eventBus)
            {
                _contexto = contexto;
                _eventBus = eventBus;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro,
                    LibreriaMaterialId = Guid.NewGuid()
                };

                _contexto.LibreriaMaterial.Add(libro);
                var valor = await _contexto.SaveChangesAsync();

                _eventBus.Publish(new EmailEventoQueue("p.andres01@hotmail.com",request.Titulo,"El contenido es un ejemplo"));

                if (valor > 0)
                {
                    return Unit.Value;
                }

                
                throw new Exception("No se puedo insertar el libro");
            }
        }
    }
}
