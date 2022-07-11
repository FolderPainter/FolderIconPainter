using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries.GetAll;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappings;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<PatchCategoryRequest, Category>().ReverseMap();

        CreateMap<CategoryDto, Category>().ReverseMap();
    }
}
