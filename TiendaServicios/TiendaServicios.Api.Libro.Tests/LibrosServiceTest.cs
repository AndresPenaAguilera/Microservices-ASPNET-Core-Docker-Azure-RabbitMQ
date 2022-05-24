﻿using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {

        private IEnumerable<LibreriaMateria> ObtenerDataPrueba() 
        {
            A.Configure<LibreriaMateria>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMateriaId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMateria>(30);
            lista[0].LibreriaMateriaId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMateria>>();

            dbSet.As<IQueryable<LibreriaMateria>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMateria>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMateria>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMateria>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMateria>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
            .Returns(new AsyncEnumerator<LibreriaMateria>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMateria>>().Setup(x => x.Provider)
                .Returns(new AsyncQueryProvider<LibreriaMateria>(dataPrueba.Provider));


            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            
            return contexto;
        }

        [Fact]
        public async void GetLibroPorId() 
        {
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg => {

                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.Ejecuta();
            request.LibroId = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMateriaId == Guid.Empty);
        }


        [Fact]
        public async void GetLibros() 
        {
            var mockContexto = CrearContexto();
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mockMapper = mapConfig.CreateMapper();

           Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mockMapper);
            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());
        }

        [Fact]
        public async void GuardarLibro() 
        {
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "Libro de Microservicio";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var libro = await new Nuevo.Manejador(contexto).Handle(request,new System.Threading.CancellationToken());

            Assert.True(libro != null);
        }
    }
}
