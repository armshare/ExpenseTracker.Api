using AutoMapper;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.Models.Dto;

namespace ExpenseTracker.Api.Mapping
{
    public class ExpenseProfile : Profile
    {

        public ExpenseProfile()
        {
            CreateMap<ExpenseCreateDto, Expense>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date ?? DateTime.UtcNow));
            CreateMap<Expense, ExpenseDto>();
        }
    }
}