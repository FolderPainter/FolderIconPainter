﻿using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Wrapper;

namespace Application.Features.Categories.Commands;
public partial class PatchCategoryRequest : IRequest<IResult<int>>
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
}

public class PatchCategoryRequestValidator : AbstractValidator<PatchCategoryRequest>
{
    public PatchCategoryRequestValidator(IUnitOfWork unitOfWork) =>
       RuleFor(p => p.Name)
           .NotEmpty()
           .MaximumLength(75)
           .MustAsync(async (category, name, ct) =>
                await unitOfWork.RepositoryClassic<Category>().Entities.FirstAsync(c => c.Name == name)
                is not Category existingCategory || existingCategory.Id == category.Id)
               .WithMessage((_, name) => $"Category {name} already Exists.");
}

internal class PatchCategoryRequestHandler : IRequestHandler<PatchCategoryRequest, IResult<int>>
{
    private readonly IUnitOfWork unitOfWork;

    public PatchCategoryRequestHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<IResult<int>> Handle(PatchCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException($"Category with id {request.Id} Not Found.");

        category.Update(request.Name);

        await unitOfWork.RepositoryClassic<Category>().UpdateAsync(category);
        await unitOfWork.CommitAsync(cancellationToken);

        return await Result<int>.SuccessAsync(request.Id);
    }
}
