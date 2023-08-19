using AutoMapper;
using Devsu.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Devsu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class MovimientosController : ControllerBase
    {
        private readonly ILogger<MovimientosController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Entities.Movimiento> _movimientoRepository;
        private readonly IRepository<Entities.Cuenta> _cuentaRepository;

        public MovimientosController(ILogger<MovimientosController> logger, IMapper mapper,
            IRepository<Entities.Movimiento> movimientoRepository, IRepository<Entities.Cuenta> cuentaRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _movimientoRepository = movimientoRepository;
            _cuentaRepository = cuentaRepository;
        }

        [HttpPost("credito")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.MovimientoResponse>> CreateCredito(DTO.Movimiento data)
        {
            try
            {
                var movimiento = _mapper.Map<Entities.Movimiento>(data);
                
                if(data.Valor <= 0)
                {
                    return BadRequest("El valor debe ser mayor a 0");
                }

                var cuenta = await _cuentaRepository.GetById(data.Cuenta);

                if(cuenta == null)
                {
                    return NotFound();
                }

                movimiento.Saldo = cuenta.Saldo + movimiento.Valor;
                movimiento.Fecha = DateTime.Today;
                movimiento.TipoMovimiento = "Credito";
                cuenta.Saldo = movimiento.Saldo;

                await _movimientoRepository.Add(movimiento);
                await _cuentaRepository .Update(cuenta);

                _logger.LogInformation("Movimiento generado", movimiento);

                var result = _mapper.Map<DTO.MovimientoResponse>(movimiento);

                return Created("", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpPost("debito")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.MovimientoResponse>> CreatDebito(DTO.Movimiento data)
        {
            try
            {
                var movimiento = _mapper.Map<Entities.Movimiento>(data);

                if (data.Valor >= 0)
                {
                    return BadRequest("El valor debe ser menor a 0");
                }

                var cuenta = await _cuentaRepository.GetById(data.Cuenta);

                if (cuenta == null)
                {
                    return NotFound();
                }


                if (cuenta.Saldo + movimiento.Valor < 0)
                {
                    return BadRequest("Saldo no disponible");
                }
                
                var movimientosHoy = await _movimientoRepository.GetAll();
                var saldoHoy = movimientosHoy
                    .Where(c => c.TipoMovimiento == "Debito" && c.Cuenta == data.Cuenta
                    && c.Fecha == DateTime.Today)
                    .Sum(c => c.Valor);
                if(Math.Abs(saldoHoy + movimiento.Valor) > 1000)
                {
                    return BadRequest("Limite Diario Excedido");
                }

                movimiento.Saldo = cuenta.Saldo + movimiento.Valor;
                movimiento.Fecha = DateTime.Today;
                movimiento.TipoMovimiento = "Debito";
                cuenta.Saldo = movimiento.Saldo;

                await _movimientoRepository.Add(movimiento);
                await _cuentaRepository.Update(cuenta);

                _logger.LogInformation("Movimiento generado", movimiento);

                var result = _mapper.Map<DTO.MovimientoResponse>(movimiento);
                return Created("", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{movimientoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteMovimiento([FromRoute]int movimientoId)
        {
            try
            {
                var movimiento = await _movimientoRepository.GetById(movimientoId);
                if(movimiento == null)
                {
                    return NotFound();
                }

                var cuenta = await _cuentaRepository.GetById(movimiento.Cuenta);
                cuenta!.Saldo += movimiento.Valor;
                await _cuentaRepository.Update(cuenta);
                await _movimientoRepository.Delete(movimiento);

                _logger.LogInformation($"Movimiento Eliminada {movimientoId}");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{movimientoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTO.MovimientoResponse>> GetMovimiento([FromRoute] int movimientoId)
        {
            try
            {
                var movimiento = await _movimientoRepository.GetById(movimientoId);
                var result = _mapper.Map<DTO.MovimientoResponse>(movimiento);
                _logger.LogInformation("Obtención de movimiento", movimiento);

                return Ok(result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<DTO.MovimientoResponse>>> ListMovimiento()
        {
            try
            {
                var listaMovimiento = await _movimientoRepository.GetAll();
                var first = listaMovimiento.FirstOrDefault();
                
                var result = _mapper.Map<List<DTO.MovimientoResponse>>(listaMovimiento);
                _logger.LogInformation("Obtención de movimiento");

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