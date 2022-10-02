using Application.Exceptions;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Queries;

public class GetCategoryRequest : IRequest<CategoryDto>
{
    public int Id { get; set; }

    public GetCategoryRequest(int id) => Id = id;
}

public class GetCategoryRequestHandler : IRequestHandler<GetCategoryRequest, CategoryDto>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCategoryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException($"Category with id {request.Id} Not Found.");
        return mapper.Map<CategoryDto>(category);
    }
}
