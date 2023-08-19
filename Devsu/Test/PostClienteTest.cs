using AutoMapper;
using Devsu.Controllers;
using Devsu.Entities;
using Devsu.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Devsu.Repository.Implementation;

namespace Devsu.Tests
{
    public class Tests
    {
        private MovimientoContext _movimientoContext;
        private ClientesController _controller;
        private IRepository<Entities.Cliente> _repositoryCliente;
        private IRepository<Entities.Persona> _repositoryPersona;
        private Mock<IMapper> _mapper;
        private Mock<ILogger<ClientesController>> _logger;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MovimientoContext>()
                .UseInMemoryDatabase("Test").Options;

            _movimientoContext = new MovimientoContext(options);
            _repositoryCliente = new Repository<Entities.Cliente>(_movimientoContext);
            _repositoryPersona = new Repository<Entities.Persona>(_movimientoContext);
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<ClientesController>>();
            _controller = new ClientesController(_logger.Object,_mapper.Object, _repositoryCliente, _repositoryPersona);
        }

        [Test]
        public async Task Add_Cliente_ResultOK()
        {
            //arr
            DTO.Cliente postParameter = new DTO.Cliente() 
            { 
                Nombre = "Test",
                Contraseña = "123123",
                Direccion = "Casa",
                Edad = 40,
                Estado = true,
                Genero = "M",
                Identificacion = "1231",
                Telefono = "123123"
            };

            var persona = new Persona
            {
                Nombre = "Test",
                Direccion = "Casa",
                Edad = 40,
                Genero = "M",
                Identificacion = "1231",
                Telefono = "123123"
            };

            var cliente = new Entities.Cliente
            {
                Contraseña = "123123",
                Estado = true
            };

            _mapper.Setup(_ => _.Map<Entities.Persona>(It.IsAny<DTO.Cliente>()))
                .Returns(persona);
            _mapper.Setup(_ => _.Map<Entities.Cliente>(It.IsAny<DTO.Cliente>()))
                .Returns(cliente);
            //act
            var result = await _controller.CreateCliente(postParameter);
            var statusCode = ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result!).StatusCode;
            //asset
            Assert.AreEqual(statusCode, 201);
        }

        [Test]
        public async Task Get_Clientes_ResultOK()
        {
            //arr
            DTO.Cliente postParameter = new DTO.Cliente()
            {
                Nombre = "Test",
                Contraseña = "123123",
                Direccion = "Casa",
                Edad = 40,
                Estado = true,
                Genero = "M",
                Identificacion = "1231",
                Telefono = "123123"
            };

            var persona = new Persona
            {
                Nombre = "Test",
                Direccion = "Casa",
                Edad = 40,
                Genero = "M",
                Identificacion = "1231",
                Telefono = "123123"
            };

            var cliente = new Entities.Cliente
            {
                Contraseña = "123123",
                Estado = true
            };

            List<DTO.Cliente> lista = new List<DTO.Cliente>()
            {
                new DTO.Cliente
                {
                    Nombre = "Test",
                    Contraseña = "123123",
                    Direccion = "Casa",
                    Edad = 40,
                    Estado = true,
                    Genero = "M",
                    Identificacion = "1231",
                    Telefono = "123123"
                }
            };

            _mapper.Setup(_ => _.Map<Entities.Persona>(It.IsAny<DTO.Cliente>()))
                .Returns(persona);
            _mapper.Setup(_ => _.Map<Entities.Cliente>(It.IsAny<DTO.Cliente>()))
                .Returns(cliente);
            _mapper.Setup(_ => _.Map<List<DTO.Cliente>>(It.IsAny<List<Entities.Cliente>>()))
                .Returns(lista);

            await _controller.CreateCliente(postParameter);
            var result = await _controller.ListarClientes();
            var statusCode = ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result!).StatusCode;
            //asset
            Assert.AreEqual(statusCode, 200);
        }
    }
}
