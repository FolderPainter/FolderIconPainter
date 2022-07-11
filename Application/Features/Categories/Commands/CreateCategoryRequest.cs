using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Categories.Commands;
public partial class CreateCategoryRequest : IRequest<IResult<int>>
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

internal class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, IResult<int>>
{
    private readonly IUnitOfWork unitOfWork;

    public CreateCategoryRequestHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<IResult<int>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name);

        await unitOfWork.RepositoryClassic<Category>().AddAsync(category, cancellationToken);
        int result = await unitOfWork.CommitAsync(cancellationToken);

        return await Result<int>.SuccessAsync(result);
    }
}
