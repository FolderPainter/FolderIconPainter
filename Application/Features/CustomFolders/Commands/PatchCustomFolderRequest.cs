using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.CustomFolders.Commands;
public partial class PatchCustomFolderRequest : IRequest<int>
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string ColorHex { get; set; }
}

public class PatchCustomFolderRequestValidator : AbstractValidator<PatchCustomFolderRequest>
{
    public PatchCustomFolderRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(p => p.Name)
           .NotEmpty()
           .MaximumLength(75)
           .MustAsync(async (customFolder, name, ct) =>
                await unitOfWork.RepositoryClassic<CustomFolder>().Entities.FirstAsync(c => c.Name == name)
                is not CustomFolder existingCustomFolder || existingCustomFolder.Id == customFolder.Id)
               .WithMessage((_, name) => $"Custom Folder {name} already Exists.");

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => $"Category with ID {id} Not Found.");

        RuleFor(p => p.ColorHex).NotEmpty().Length(7);
    }
}

internal class PatchCustomFolderRequestHandler : IRequestHandler<PatchCustomFolderRequest, int>
{
    private readonly IUnitOfWork unitOfWork;

    public PatchCustomFolderRequestHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(PatchCustomFolderRequest request, CancellationToken cancellationToken)
    {
        var customFolder = await unitOfWork.RepositoryClassic<CustomFolder>().GetByIdAsync(request.Id, cancellationToken);

        _ = customFolder ?? throw new NotFoundException($"Custom Folder with id {request.Id} Not Found.");
        customFolder.Update(request.Name, request.CategoryId, request.ColorHex);

        await unitOfWork.RepositoryClassic<CustomFolder>().UpdateAsync(customFolder);
        return await unitOfWork.CommitAsync(cancellationToken);
    }
}
