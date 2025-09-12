using AutoMapper;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.Models.Dto;

namespace ExpenseTracker.Api.Mapping
{
    public class ExpenseProfile : Profile
    {

        public ExpenseProfile()
        {
            CreateMap<Expense, ExpenseDto>();
            CreateMap<ExpenseCreateDto, Expense>();
        }
    }
}