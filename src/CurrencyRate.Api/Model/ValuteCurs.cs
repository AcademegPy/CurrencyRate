namespace CurrencyRate.Api.Model
{
    public class ValuteCurs
    {
        public string Vname { get; set; }
        public decimal Vnom { get; set; }
        public decimal Vcurs { get; set; }
        public int Vcode { get; set; }
        public string VchCode { get; set; }
        public double VunitRate { get; set; }
    }
}
