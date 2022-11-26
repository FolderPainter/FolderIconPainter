using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Client.Infrastructure.Services;

public class CategoryService : BaseService
{
    private readonly IValidator<CreateCategoryRequest> _createValidator;

    public CategoryService(IMediator mediator, IValidator<CreateCategoryRequest> validator) : base(mediator)
    {
        _createValidator = validator;
    }

    public Task<IEnumerable<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    {
        return _mediator.Send(request);
    }


    //[HttpPost("search")]
    //public Task<PaginationResponse<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    //{
    //    return _mediator.Send(request);
    //}

    public Task<CategoryDto> GetAsync(int id)
    {
        return _mediator.Send(new GetCategoryRequest(id));
    }

    public async Task<int> CreateAsync(CreateCategoryRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            return await _mediator.Send(request);
        }
        return 0;
    }

    //[HttpPut("{id:int}")]
    //public async Task<ActionResult<int>> UpdateAsync(PatchCategoryRequest request, int id)
    //{
    //    return id != request.Id
    //        ? BadRequest()
    //        : Ok(await _mediator.Send(request));
    //}

    public Task<int> DeleteAsync(int id)
    {
        return _mediator.Send(new DeleteCategoryRequest(id));
    }
}
