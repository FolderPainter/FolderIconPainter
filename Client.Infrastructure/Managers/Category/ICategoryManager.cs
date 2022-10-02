using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Category;

public interface ICategoryManager: IManager
{
    public Task<CategoryDto> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
    
    public Task<PaginatedResult<CategoryDto>> SearchCategoriesAsync(SearchCategoriesRequest request, CancellationToken cancellationToken = default);
    
    public Task<int> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default);

    public Task<int> PatchCategoryAsync(PatchCategoryRequest patchCategoryRequest, CancellationToken cancellationToken = default);

    public Task<int> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
}
