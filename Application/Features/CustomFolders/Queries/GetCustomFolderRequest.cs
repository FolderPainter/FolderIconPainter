using Application.Exceptions;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.CustomFolders.Queries;

public class GetCustomFolderRequest : IRequest<CustomFolderDto>
{
    public int Id { get; set; }

    public GetCustomFolderRequest(int id) => Id = id;
}

public class GetCustomFolderRequestHandler : IRequestHandler<GetCustomFolderRequest, CustomFolderDto>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCustomFolderRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<CustomFolderDto> Handle(GetCustomFolderRequest request, CancellationToken cancellationToken)
    {
        var customFolder = await unitOfWork.RepositoryClassic<CustomFolder>().GetByIdAsync(request.Id, cancellationToken);

        _ = customFolder ?? throw new NotFoundException($"Custom Folder with id {request.Id} Not Found.");
        return mapper.Map<CustomFolderDto>(customFolder);
    }
}
