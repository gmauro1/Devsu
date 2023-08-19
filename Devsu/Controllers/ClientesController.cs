using AutoMapper;
using Devsu.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Devsu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ClientesController : ControllerBase
    {
        private readonly ILogger<ClientesController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Entities.Persona> _repositoryPersona;
        private readonly IRepository<Entities.Cliente> _repositoryCliente;

        public ClientesController(ILogger<ClientesController> logger, IMapper mapper, IRepository<Entities.Cliente> clienteRepository, IRepository<Entities.Persona> personaRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryCliente = clienteRepository;
            _repositoryPersona = personaRepository;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cliente>> CreateCliente(DTO.Cliente data)
        {
            try
            {
                var persona = _mapper.Map<Entities.Persona>(data);
                var cliente = _mapper.Map<Entities.Cliente>(data);
                var personaResult = await _repositoryPersona.Add(persona);
                cliente.Persona = personaResult.Id;
                var clienteResult = await _repositoryCliente.Add(cliente);

                _logger.LogInformation("Cliente generado", clienteResult);

                return Created("", data);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{clienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cliente>> DeleteCliente([FromRoute]int clienteId)
        {
            try
            {
                var cliente = await _repositoryCliente.GetById(clienteId);
                if(cliente == null)
                {
                    return NotFound();
                }

                await _repositoryCliente.Delete(cliente);

                _logger.LogInformation($"Cliente Eliminado {clienteId}");

                return Ok();

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpPut("{clienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cliente>> UpdateCliente([FromRoute] int clienteId, [FromBody] DTO.Cliente data)
        {
            try
            {
                var persona = _mapper.Map<Entities.Persona>(data);
                var clienteUpdate = await _repositoryCliente.GetById(clienteId);
                if (clienteUpdate == null)
                {
                    return NotFound();
                }

                clienteUpdate.Estado = data.Estado;
                clienteUpdate.Contraseña = data.Contraseña;
                persona.Id = clienteUpdate.Persona;
                await _repositoryCliente.Update(clienteUpdate);
                await _repositoryPersona.Update(persona);


                _logger.LogInformation("Cliente actualizado", data);

                return Ok(data);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{clienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cliente>> GeCliente([FromRoute] int clienteId)
        {
            try
            {
                Expression<Func<Entities.Cliente, bool>> where = c => c.Id == clienteId;
                var cliente = await _repositoryCliente.GetById(clienteId, "PersonaNavigation",
                    where);
                var result = _mapper.Map<DTO.Cliente>(cliente);
                _logger.LogInformation("Obtención de cliente", cliente);

                return Ok(result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<DTO.Cliente>>> ListarClientes()
        {
            try
            {
                var listaCliente = await _repositoryCliente.GetAll(new List<string> { "PersonaNavigation" });
                var result = _mapper.Map<List<DTO.Cliente>>(listaCliente);
                _logger.LogInformation("Obtención de clientes");

                return Ok(result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
    }
}