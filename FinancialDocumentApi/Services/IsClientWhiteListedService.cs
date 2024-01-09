using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialDocumentApi.Services
{
    public class IsClientWhiteListedService : IIsClientWhiteListedService
    {
        private readonly IClientRepository _clientRepository;

        public IsClientWhiteListedService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<bool> IsClientIdWhitelistedAsync(int tenantId, int clientId)
        {
            return await _clientRepository.IsClientIdWhitelistedAsync(tenantId, clientId);
        }
    }
}