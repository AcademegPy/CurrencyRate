using System;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Services
{
    public interface IValuteCursService<T>
    {
        Task<ServiceResult<T>> GetCurs(DateTime? date, int? code);
    }
}
