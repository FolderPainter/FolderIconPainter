using Domain.Enums;
using Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Services;

namespace Application.Features.CustomFolder.Queries.GetAllPaged;
public class GetAllPagedCustomFoldersQuery : IRequest<PaginatedResult<GetAllPagedCustomFoldersResponse>>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string SearchString { get; set; }

    public SortDirection SortDirection { get; set; }

    public GetAllPagedCustomFoldersQuery(int pageNumber, int pageSize, string searchString, SortDirection sortDirection)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchString = searchString;
        SortDirection = sortDirection;
    }
}

internal class GetAllPagedShopItemsQueryHandler : IRequestHandler<GetAllPagedCustomFoldersQuery, PaginatedResult<GetAllPagedCustomFoldersResponse>>
{
    private readonly ICustomFolderService customFolderService;

    public GetAllPagedShopItemsQueryHandler(ICustomFolderService customFolderService)
    {
        this.customFolderService = customFolderService;
    }

    public async Task<PaginatedResult<GetAllPagedCustomFoldersResponse>> Handle(GetAllPagedCustomFoldersQuery request, CancellationToken cancellationToken)
    {
        return await customFolderService.GetAllPagedShopItems(request, cancellationToken);
    }
}
