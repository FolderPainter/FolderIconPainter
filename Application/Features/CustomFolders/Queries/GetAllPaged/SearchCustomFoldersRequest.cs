using Domain.Enums;
using Shared.Wrapper;
using MediatR;
using Application.Interfaces.Services;
using Application.Specifications;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Features.CustomFolders.Queries.GetAllPaged;

public class SearchCustomFoldersRequest : IRequest<PaginatedResult<CustomFolderDto>>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string SearchString { get; set; }

    public SortDirection SortDirection { get; set; }

    public SearchCustomFoldersRequest(int pageNumber, int pageSize, string searchString, SortDirection sortDirection)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchString = searchString;
        SortDirection = sortDirection;
    }
}

internal class GetAllPagedShopItemsQueryHandler : IRequestHandler<SearchCustomFoldersRequest, PaginatedResult<CustomFolderDto>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetAllPagedShopItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<PaginatedResult<CustomFolderDto>> Handle(SearchCustomFoldersRequest request, CancellationToken cancellationToken)
    {
        bool? isSortDescending = null;

        if (request.SortDirection == SortDirection.Ascending)
            isSortDescending = true;
        else if (request.SortDirection == SortDirection.Descending)
            isSortDescending = false;

        CustomFolderFilterSpecification specification = new CustomFolderFilterSpecification(request.SearchString);
        var data = unitOfWork
            .RepositoryClassic<CustomFolder>()
            .Entities
            .Include(element => element.Category)
            .Specify(specification)
            .ProjectTo<CustomFolderDto>(mapper.ConfigurationProvider);

        data = isSortDescending.Value ? data.OrderByDescending(element => element.Name)
              : data.OrderBy(element => element.Name);

        return await data.ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
