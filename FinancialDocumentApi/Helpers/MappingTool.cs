using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using FinancialDocumentApi.DTOs;

namespace Infrastructure.Helpers
{
    public class MappingTool : Profile
    {
        public MappingTool(){
            CreateMap<FinancialDocument, FinancialDocumentDTO>()
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions))
            .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.Tenant));

        CreateMap<Transaction, TransactionDTO>();

        CreateMap<Tenant, TenantDTO>();
        }
    }
}