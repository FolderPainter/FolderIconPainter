using Application.Features.Category.Commands;
using Application.Features.Category.Queries.GetAll;
using Domain.Entities;

namespace Application.Interfaces.Services;
public interface ICategoryService
{
    Task<bool> CreateCategoryAsync(CreateCategoryCommand command, CancellationToken cancellationToken);

    Task<bool> PatchCategoryAsync(PatchCategoryCommand command, CancellationToken cancellationToken);

    Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken);

    Task<Category> GetCategoryById(int id, CancellationToken cancellationToken);

    Task<IEnumerable<GetAllCategoriesResponse>> GetAllCategories(CancellationToken cancellationToken);
}
