using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialDocumentApi.Services
{
    public interface IProductValidationService
    {
        Task<bool> IsProductSupportedAsync(string productCode);
    }
}