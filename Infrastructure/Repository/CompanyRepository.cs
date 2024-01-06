using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        public Task<Company> GetCompanyByClientVATAsync(string clientVAT)
        {
            throw new NotImplementedException();
        }
    }
}