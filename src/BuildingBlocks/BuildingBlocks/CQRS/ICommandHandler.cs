using MediatR;

namespace BuildingBlocks.CQRS
{
    // in = sadece parametre tipinde kullanilabilir return olamaz 
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse> where TResponse : notnull
    {

    }

}
