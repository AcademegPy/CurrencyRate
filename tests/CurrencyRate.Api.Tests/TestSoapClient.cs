using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using CurrencyRate.Api.Model;
using WebServiceReference;

namespace CurrencyRate.Api.Tests
{
    internal class TestSoapClient : DailyInfoSoapClient
    {
        public TestSoapClient(EndpointConfiguration endpointConfiguration) : base(endpointConfiguration)
        {
        }
        public override Task<GetCursOnDateResponse> GetCursOnDateAsync(GetCursOnDateRequest request)
        {
            var dataSet = new DataSet();
            DataTable table = new DataTable("ValuteCursOnDate");
            dataSet.Tables.Add(table);
            DataColumn vNameColumn = new DataColumn("Vname", typeof(string));
            DataColumn vNomColumn = new DataColumn("Vnom", typeof(string));
            DataColumn vCursColumn = new DataColumn("Vcurs", typeof(string));
            DataColumn vCodeColumn = new DataColumn("Vcode", typeof(string));
            DataColumn vchCodeColumn = new DataColumn("VchCode", typeof(string));
            DataColumn vunitRateColumn = new DataColumn("VunitRate", typeof(string));

            table.Columns.AddRange(new DataColumn[]
            {
                vNameColumn, vNomColumn, vCursColumn,
                vCodeColumn, vchCodeColumn, vunitRateColumn
            });

            var rates = new ValuteCurs[]
            {
                new ValuteCurs
                {
                    Vname = "Австралийский доллар",
                    Vnom = 1,
                    Vcurs = 52.2772M,
                    Vcode = 36,
                    VchCode = "AUD",
                    VunitRate = 52.2772
                },
                new ValuteCurs
                {
                    Vname = "Азербайджанский мана",
                    Vnom = 1,
                    Vcurs = 48.4119M,
                    Vcode = 944,
                    VchCode = "AZN",
                    VunitRate = 48.4119
                },
                new ValuteCurs
                {
                    Vname = "Фунт стерлингов",
                    Vnom = 1,
                    Vcurs = 108.5294M,
                    Vcode = 826,
                    VchCode = "GBP",
                    VunitRate = 108.5294
                }
            };


            foreach (var rate in rates)
            {
                var row = table.NewRow();
                row["Vname"] = rate.Vname;
                row["Vnom"] = rate.Vnom;
                row["Vcurs"] = rate.Vcurs;
                row["Vcode"] = rate.Vcode;
                row["VchCode"] = rate.VchCode;
                row["VunitRate"] = rate.VunitRate;

                table.Rows.Add(row);
            }

            return Task.FromResult<GetCursOnDateResponse>(new GetCursOnDateResponse { GetCursOnDateResult = dataSet });
        }
    }
}
