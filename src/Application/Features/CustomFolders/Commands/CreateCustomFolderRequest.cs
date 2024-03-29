﻿using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomFolders.Commands;
public partial class CreateCustomFolderRequest : IRequest<int>
{
    public string Name { get; set; }

    public int CategoryId { get; set; }

    public string ColorHex { get; set; }
}

public class CreateCustomFolderRequestValidator : AbstractValidator<CreateCustomFolderRequest>
{
    public CreateCustomFolderRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => !await unitOfWork.RepositoryClassic<CustomFolder>().Entities.AnyAsync(c => c.Name == name, ct))
                .WithMessage((_, name) => $"Custom Folder {name} already Exists.");

        RuleFor(p => p.CategoryId)
        .NotEmpty()
        .MustAsync(async (id, ct) => await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => $"Category with ID {id} Not Found.");

        RuleFor(p => p.ColorHex).NotEmpty().Length(7);
    }
}

internal class CreateCustomFolderRequestHandler : IRequestHandler<CreateCustomFolderRequest, int>
{
    private readonly IUnitOfWork unitOfWork;

    public CreateCustomFolderRequestHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateCustomFolderRequest request, CancellationToken cancellationToken)
    {
        var customFolder = new CustomFolder(request.Name, request.CategoryId, request.ColorHex);

        await unitOfWork.RepositoryClassic<CustomFolder>().AddAsync(customFolder, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return customFolder.Id;
    }
}
