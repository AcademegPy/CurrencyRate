using System.Collections.Generic;
using System;

namespace CurrencyRate.Api.Services
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public Exception Exception { get; set; }
    }
}
