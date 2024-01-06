using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialDocumentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        
        public TenantController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        [HttpGet("{tenantId}/whitelisted")]
        public async Task<IActionResult> IsWhitelisted(int tenantId)
        {
            var isWhitelisted = await _tenantRepository.IsTenantWhitelistedAsync(tenantId);
            return Ok(new { TenantId = tenantId, IsWhitelisted = isWhitelisted });
        }
    }
}