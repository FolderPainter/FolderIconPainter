using Application.Features.Category.Commands;
using Application.Features.Category.Queries.GetAll;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappings;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryCommand, Category>().ReverseMap();

        CreateMap<PatchCategoryCommand, Category>().ReverseMap();

        CreateMap<GetAllCategoriesResponse, Category>().ReverseMap();
    }
}
