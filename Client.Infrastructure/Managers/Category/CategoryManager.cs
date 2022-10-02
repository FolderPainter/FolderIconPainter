using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using Shared.Wrapper;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers.Category;

public class CategoryManager : ICategoryManager
{
    private readonly HttpClient _httpClient;

    public CategoryManager(HttpClient httpClient) => _httpClient = httpClient;

    public Task<int> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<CategoryDto> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> PatchCategoryAsync(PatchCategoryRequest patchCategoryRequest, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<PaginatedResult<CategoryDto>> SearchCategoriesAsync(SearchCategoriesRequest request, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}
