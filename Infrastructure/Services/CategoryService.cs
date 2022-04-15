using Application.Exceptions;
using Application.Features.Category.Commands;
using Application.Features.Category.Queries.GetAll;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<bool> CreateCategoryAsync(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        if (await unitOfWork.RepositoryClassic<Category>().Entities
           .AnyAsync(p => p.Name == command.Name, cancellationToken))
        {
            throw new ApiException("Category already exists!");
        }

        await unitOfWork.RepositoryClassic<Category>().AddAsync(mapper.Map<Category>(command), cancellationToken);

        int resultEntitySave = await unitOfWork.CommitAsync(cancellationToken);
        return resultEntitySave != 0;
    }

    public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            return false;
        }
        await unitOfWork.RepositoryClassic<Category>().DeleteAsync(category);
        return await unitOfWork.CommitAsync(cancellationToken) != 0;
    }

    public async Task<bool> PatchCategoryAsync(PatchCategoryCommand command, CancellationToken cancellationToken)
    {
        if (await unitOfWork.RepositoryClassic<Category>().Entities
            .AnyAsync(p => p.Name == command.Name, cancellationToken))
        {
            throw new ApiException("Category already exists!");
        }

        var category = await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(command.Id, cancellationToken);
        if (category != null)
        {
            category.Name = command.Name ?? category.Name;

            await unitOfWork.RepositoryClassic<Category>().UpdateAsync(category);
            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<Category> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException(nameof(Category), id);
        }
        return category;
    }

    public async Task<IEnumerable<GetAllCategoriesResponse>> GetAllCategories(CancellationToken cancellationToken)
    {
        var categories = unitOfWork.RepositoryClassic<Category>().Entities
            .ProjectTo<GetAllCategoriesResponse>(mapper.ConfigurationProvider);
        
        return await categories.ToListAsync(cancellationToken);
    }
}
