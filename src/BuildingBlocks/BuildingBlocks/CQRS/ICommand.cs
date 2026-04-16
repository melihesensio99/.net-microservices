using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommand : ICommand<Unit>  //hicbir sey donmuyorum
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse> //out = sadece return donus tipinde kullanilabilir 
    {

    }
}