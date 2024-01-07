using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace FinancialDocumentApi.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Company> GetCompanyByClientVATAsync(string clientVAT)
        {
            return await _companyRepository.GetCompanyByClientVATAsync(clientVAT);
        }
    }
}