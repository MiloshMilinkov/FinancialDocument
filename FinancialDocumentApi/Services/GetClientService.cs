using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace FinancialDocumentApi.Services.Interfaces
{
    public class GetClientService : IGetClientService
    {
        private readonly IClientRepository _clientRepository;

        public GetClientService(IClientRepository clientRepository)
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
    }
}