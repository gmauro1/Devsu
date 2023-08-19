using AutoMapper;
using Devsu.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Devsu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ReportesController : ControllerBase
    {
        private readonly ILogger<ReportesController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Entities.Movimiento> _movimientoRepository;
        private readonly IRepository<Entities.Cuenta> _cuentaRepository;

        public ReportesController(ILogger<ReportesController> logger, IMapper mapper,
            IRepository<Entities.Movimiento> movimientoRepository, IRepository<Entities.Cuenta> cuentaRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _movimientoRepository = movimientoRepository;
            _cuentaRepository = cuentaRepository;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<DTO.Reporte>>> GetReporte([FromQuery] InputReporte inputReporte)
        {
            try
            {
                Expression<Func<Entities.Movimiento, bool>> where =
                    c => c.Fecha >= inputReporte.Desde && c.Fecha <= inputReporte.Hasta;
                var includes = new List<string>
                {
                    "CuentaNavigation",
                    "CuentaNavigation.ClienteNavigation",
                    "CuentaNavigation.ClienteNavigation.PersonaNavigation"
                };
                var movimiento = await _movimientoRepository.GetAll(includes, where);
                var result = movimiento.Select(c => new DTO.Reporte
                {
                    Cliente = c.CuentaNavigation.ClienteNavigation.PersonaNavigation.Nombre,
                    Estado = c.CuentaNavigation.Estado??false,
                    Fecha = c.Fecha.ToShortDateString(),
                    Tipo = c.TipoMovimiento,
                    Movimiento = c.Valor,
                    NumeroCuenta = c.CuentaNavigation.NumeroCuenta,
                    SaldoDisponible = c.Saldo,
                    SaldoInicial = c.Saldo - c.Valor,
                }).ToList();

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