using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialDocumentApi.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<Client> GetClientAsync(int tenantId, int documentId)
        {
            return await _clientRepository.GetClientAsync(tenantId, documentId);
        }

        public async Task<bool> IsClientIdWhitelistedAsync(int clientId)
        {
            return await _clientRepository.IsClientIdWhitelistedAsync(clientId);
        }
    }
}