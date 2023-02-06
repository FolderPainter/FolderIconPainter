using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Queries
{
    public class SearchCategoriesRequest : IRequest<IEnumerable<CategoryDto>>
    {
        public SearchCategoriesRequest(SortDirection sortDirection = SortDirection.Ascending)
        {
            SortDirection = sortDirection;
        }

        public SortDirection SortDirection { get; set; }
    }

    internal class SearchCategoriesRequestHandler : IRequestHandler<SearchCategoriesRequest, IEnumerable<CategoryDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SearchCategoriesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
      
        public async Task<IEnumerable<CategoryDto>> Handle(SearchCategoriesRequest request, CancellationToken cancellationToken)
        {
            var categories = unitOfWork.RepositoryClassic<Category>().Entities
                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider);

            bool? isSortDescending = null;

            if (request.SortDirection == SortDirection.Ascending)
                isSortDescending = true;
            else if (request.SortDirection == SortDirection.Descending)
                isSortDescending = false;

            categories = isSortDescending.Value ? 
                categories.OrderByDescending(element => element.Name): categories.OrderBy(element => element.Name);

            return await categories.ToListAsync(cancellationToken);
        }
    }
}
