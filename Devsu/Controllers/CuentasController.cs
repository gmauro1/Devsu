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
    public class CuentasController : ControllerBase
    {
        private readonly ILogger<CuentasController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Entities.Cuenta> _repositoryCuenta;

        public CuentasController(ILogger<CuentasController> logger, IMapper mapper, IRepository<Entities.Cuenta> cuentaRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryCuenta = cuentaRepository;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cuenta>> CreateCuenta(DTO.Cuenta data)
        {
            try
            {
                var cuenta = _mapper.Map<Entities.Cuenta>(data);
                await _repositoryCuenta.Add(cuenta);

                _logger.LogInformation("Cuenta generada", cuenta);

                return Created("", data);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{cuentaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cuenta>> DeleteCuenta([FromRoute]int cuentaId)
        {
            try
            {
                var cuenta = await _repositoryCuenta.GetById(cuentaId);
                if(cuenta == null)
                {
                    return NotFound();
                }

                await _repositoryCuenta.Delete(cuenta);

                _logger.LogInformation($"Cuenta Eliminada {cuentaId}");

                return Ok();

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpPut("{cuentaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cuenta>> UpdateCuenta([FromRoute] int cuentaId, [FromBody] DTO.Cuenta data)
        {
            try
            {
                var cuentaUpdate = await _repositoryCuenta.GetById(cuentaId);
                if (cuentaUpdate == null)
                {
                    return NotFound();
                }

                cuentaUpdate.Saldo = data.Saldo;
                cuentaUpdate.NumeroCuenta = data.NumeroCuenta;
                cuentaUpdate.TipoCuenta = data.TipoCuenta;
                cuentaUpdate.Estado = data.Estado; 
                
                await _repositoryCuenta.Update(cuentaUpdate);

                _logger.LogInformation("Cuenta actualizada", data);

                return Ok(data);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{cuentaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.Cuenta>> GetCuenta([FromRoute] int cuentaId)
        {
            try
            {
                var cuenta = await _repositoryCuenta.GetById(cuentaId);
                var result = _mapper.Map<DTO.Cuenta>(cuenta);
                _logger.LogInformation("Obtención de cuenta", cuenta);

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
        public async Task<ActionResult<List<DTO.Cuenta>>> ListCuenta()
        {
            try
            {
                var listaCuenta = await _repositoryCuenta.GetAll();
                var result = _mapper.Map<List<DTO.Cuenta>>(listaCuenta);
                _logger.LogInformation("Obtención de cuenta");

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