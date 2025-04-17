using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRate.Api.Model;
using CurrencyRate.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CurrencyRate.Api.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class ValuteCursController : ControllerBase
    {
        private readonly IValuteCursService<IEnumerable<ValuteCurs>> _valuteCursService;
        private readonly ILogger<ValuteCursController> _logger;

        public ValuteCursController(IValuteCursService<IEnumerable<ValuteCurs>> valuteCursService, ILogger<ValuteCursController> logger = null)
        {
            _valuteCursService = valuteCursService;
            _logger = logger;
        }

        /// <summary>
        /// Получение курса валют
        /// </summary>
        /// <param name="date">Дата курса</param>
        /// <param name="code">Код валюты</param>
        /// <returns>Список курса валют</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="204">Данные отсутсвуют</response>
        [HttpGet]
        [Route("rates")]
        public async Task<ActionResult<IEnumerable<ValuteCurs>>> GetRates(DateTime? date, int? code)
        {
            _logger?.LogInformation("Запрос Дата={Date}, Code={code}", date, code);

            var result = await _valuteCursService.GetCurs(date, code);

            if (result.Exception != null)
            {
                return StatusCode(500, "Ошибка обработки запроса");
            }

            if (result.Data == null)
            {
                _logger?.LogInformation("Данные для Дата={Date}, Code={code} не найдены", date, code);
                return NoContent();
            }

            _logger?.LogInformation("Найдено {Count} результатов для Дата={Date}, Code={code}", result.Data.Count(), date, code);

            return Ok(result.Data);
        }
    }
}
