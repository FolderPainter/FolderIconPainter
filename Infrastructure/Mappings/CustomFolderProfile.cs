using Application.Features.CustomFolder.Commands;
using Application.Features.CustomFolder.Queries.GetAllPaged;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappings;
public class CustomFolderProfile : Profile
{
    public CustomFolderProfile()
    {
        CreateMap<CreateCustomFolderCommand, CustomFolder>().ReverseMap();

        CreateMap<PatchCustomFolderCommand, CustomFolder>().ReverseMap();

        CreateMap<CustomFolder, GetAllPagedCustomFoldersResponse>().ReverseMap();
    }
}
