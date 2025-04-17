using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRate.Api.Model;
using Microsoft.Extensions.Logging;
using WebServiceReference;

namespace CurrencyRate.Api.Services
{
    public class ValuteCursService : IValuteCursService<IEnumerable<ValuteCurs>>
    {
        private readonly DailyInfoSoapClient _client;
        private readonly ILogger<ValuteCursService> _logger;

        public ValuteCursService(DailyInfoSoapClient dailyInfoSoapClient, ILogger<ValuteCursService> logger = null)
        {
            _client = dailyInfoSoapClient;
            _logger = logger;
        }

        public async Task<ServiceResult<IEnumerable<ValuteCurs>>> GetCurs(DateTime? date, int? code)
        {
            try
            {
                if (date == null)
                { 
                    date = DateTime.Now;
                }

                var request = new GetCursOnDateRequest { On_date = (DateTime)date };
                var resp = await _client.GetCursOnDateAsync(request);
                var table = resp.GetCursOnDateResult.Tables["ValuteCursOnDate"];

                if (table.Rows.Count == 0)
                {
                    return new ServiceResult<IEnumerable<ValuteCurs>> { Data = null };
                }

                if (code != null)
                {
                    var curs = GetSpecifiedValuteCurs(table, code);
                    if (curs != null)
                    {
                        return new ServiceResult<IEnumerable<ValuteCurs>> { Data = curs };
                    }

                    return new ServiceResult<IEnumerable<ValuteCurs>>();
                }

                ValuteCurs[] curses = GetAllCurses(table);

                return new ServiceResult<IEnumerable<ValuteCurs>> { Data = curses };
            }
            catch (Exception ex)
            {
                _logger?.LogError("Ошибка обработки запроса {Excepiton}", ex);
                return new ServiceResult<IEnumerable<ValuteCurs>> { Exception = ex };
            }
        }

        private ValuteCurs[] GetAllCurses(DataTable table)
        {
            var rows = table.Rows;
            var curses = new ValuteCurs[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                curses[i] = GetValuteCursFromRow(rows[i]);
            }

            return curses;
        }

        private IEnumerable<ValuteCurs> GetSpecifiedValuteCurs(DataTable table, int? code)
        {
            var valuteCursRow = table.Select($"Vcode = '{code}'").FirstOrDefault();
            if (valuteCursRow == null)
            {
                return null;
            }

            return new ValuteCurs[]
            {
                GetValuteCursFromRow(valuteCursRow)
            };

        }

        private ValuteCurs GetValuteCursFromRow(DataRow row)
        {
            return new ValuteCurs
            {
                Vname = row["Vname"].ToString().TrimEnd(),
                Vnom = decimal.Parse(row["Vnom"].ToString()),
                Vcurs = decimal.Parse(row["Vcurs"].ToString()),
                Vcode = int.Parse(row["Vcode"].ToString()),
                VchCode = row["VchCode"].ToString(),
                VunitRate = double.Parse(row["VunitRate"].ToString())
            };
        }
    }
}
