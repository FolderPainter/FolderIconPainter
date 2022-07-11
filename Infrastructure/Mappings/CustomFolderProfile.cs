using Application.Features.CustomFolders.Commands;
using Application.Features.CustomFolders.Queries.GetAllPaged;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappings;
public class CustomFolderProfile : Profile
{
    public CustomFolderProfile()
    {
        CreateMap<CreateCustomFolderRequest, CustomFolder>().ReverseMap();

        CreateMap<PatchCustomFolderRequest, CustomFolder>().ReverseMap();

        CreateMap<CustomFolder, CustomFolderDto>().ReverseMap();
    }
}
