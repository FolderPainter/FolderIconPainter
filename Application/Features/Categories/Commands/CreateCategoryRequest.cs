using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Categories.Commands;
public partial class CreateCategoryRequest : IRequest<int>
{
    [Required]
    public string Name { get; set; }
}

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(IUnitOfWork unitOfWork) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => !await unitOfWork.RepositoryClassic<Category>().Entities.AnyAsync(c => c.Name == name, ct))
                .WithMessage((_, name) => $"Category {name} already Exists.");
}

internal class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, int>
{
    private readonly IUnitOfWork unitOfWork;

    public CreateCategoryRequestHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<int> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name);

        await unitOfWork.RepositoryClassic<Category>().AddAsync(category, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return category.Id;
    }
}
