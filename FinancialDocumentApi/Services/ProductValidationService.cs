using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;

namespace FinancialDocumentApi.Services
{
    public class ProductValidationService : IProductValidationService
    {
        private readonly IProductRepository _productRepository;

        public ProductValidationService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> IsProductSupportedAsync(string productCode)
        {
            if(string.IsNullOrWhiteSpace(productCode)){
                return false;
            }
            var product = await _productRepository.GetProductByCodeAsync(productCode);

            return product !=null && product.IsSupported;
        }
    }
}