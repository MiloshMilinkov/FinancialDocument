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
                                .FirstOrDefaultAsync(fd => fd.TenantId == tenantId && fd.Id == documentId);
        }

        public async Task<FinancialDocument> AnonymizeFinancialDocumentAsync(FinancialDocument document, string productCode)
        {
            // Simplified anonymization logic. Update this based on your actual requirements.
            if (document != null)
            {
                document.AccountNumber = HashValue(document.AccountNumber); // Example of anonymization
                foreach( var tran in document.Transactions){
                    tran.TransactionId = "####";
                }
                // Apply further anonymization as needed
            }

            return document;
        }

        private string HashValue(string value)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
                return Convert.ToHexString(hashBytes); // Converts the byte array to a hex string
            }
        }
    }
}