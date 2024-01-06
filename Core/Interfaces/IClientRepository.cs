using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IClientRepository
    {
        Task<Client>  GetClientDetailsIfWhitelisted(Guid tenantId, Guid documentId);
    }
}