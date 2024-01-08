using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace FinancialDocumentApi.Services
{
    public interface ICompanyService
    {
         Task<(string RegistrationNumber, string CompanyType)?> GetCompanyByClientVATAsync(string clientVAT);
    }
}