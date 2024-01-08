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

        public async Task<(string RegistrationNumber, string CompanyType)?> GetCompanyByClientVATAsync(string clientVAT)
        {
            var company = await  _companyRepository.GetCompanyByClientVATAsync(clientVAT);
            if(company != null){
                return (company.RegistrationNumber, company.CompanyType);
            }
            return null;
        }
    }
}