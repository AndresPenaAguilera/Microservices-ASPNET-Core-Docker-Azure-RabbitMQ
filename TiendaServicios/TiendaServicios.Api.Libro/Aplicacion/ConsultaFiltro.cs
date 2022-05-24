using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{

    public class ConsultaFiltro
    {

        public class Ejecuta : IRequest<LibroMaterialDto>
        {
            public Guid? LibroId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, LibroMaterialDto>
        {

            public readonly ContextoLibreria _contexto;
            public readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<LibroMaterialDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriaMaterial.Where(x => x.LibreriaMateriaId == request.LibroId).FirstOrDefaultAsync();
                var libroDto = _mapper.Map<LibreriaMateria, LibroMaterialDto>(libro);
                return libroDto;
            }
        }
    }
}
