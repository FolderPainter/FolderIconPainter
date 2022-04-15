using Application.Interfaces.Services;
using Domain.Enums;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Category.Queries.GetAll
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<GetAllCategoriesResponse>>
    {
        public GetAllCategoriesQuery(SortDirection sortDirection)
        {
            SortDirection = sortDirection;
        }

        public SortDirection SortDirection { get; set; }
    }

    internal class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<GetAllCategoriesResponse>>
    {
        private readonly ICategoryService categoryService;

        public GetAllCategoriesQueryHandler(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IEnumerable<GetAllCategoriesResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await categoryService.GetAllCategories(cancellationToken);
        }
    }
}
