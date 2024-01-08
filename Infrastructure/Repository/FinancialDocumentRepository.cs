using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class FinancialDocumentRepository : IFinancialDocumentRepository
    {
        private readonly AppDbContext _context;

        public FinancialDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FinancialDocument> GetFinancialDocumentAsync(int tenantId, int documentId)
        {
            return await _context.FinancialDocuments
                                .Include(fd => fd.Transactions)
                                .Include(fd => fd.Tenant)
                                .Include(fd => fd.Transactions)
                                .Include(fd => fd.Product)
                                .FirstOrDefaultAsync(fd => fd.TenantId == tenantId && fd.Id == documentId);
        }
    }
}