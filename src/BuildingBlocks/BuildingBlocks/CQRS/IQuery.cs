using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface IQuery : IRequest<Unit> //unit==void
    {
    }
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
