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
        public async Task<(int ClientId, string ClientVAT)?> GetClientAsync(int tenantId, int documentId)
        {
            var client = await _clientRepository.GetClientAsync(tenantId, documentId);
            if (client != null)
            {
                return (client.Id, client.ClientVAT);
            }
            return null;
        }

        public async Task<bool> IsClientIdWhitelistedAsync(int tenantId, int clientId)
        {
            return await _clientRepository.IsClientIdWhitelistedAsync(tenantId, clientId);
        }
    }
}