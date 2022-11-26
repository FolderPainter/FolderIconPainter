using MediatR;

namespace Client.Infrastructure.Services;

public class BaseService 
{
    protected IMediator _mediator { get; set; }

    public BaseService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException();
    }
}
