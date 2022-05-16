using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;


namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Consulta
    {


        public class ListLibros : IRequest<List<LibroMaterialDto>>
        {

        }

        public class Manejador : IRequestHandler<ListLibros, List<LibroMaterialDto>>
        {

            public readonly ContextoLibreria _contexto;
            public readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<LibroMaterialDto>> Handle(ListLibros request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriaMaterial.ToListAsync();
                var librosDto = _mapper.Map<List<LibreriaMateria>, List<LibroMaterialDto>>(libros);
                return librosDto;
            }
        }
    }
}
