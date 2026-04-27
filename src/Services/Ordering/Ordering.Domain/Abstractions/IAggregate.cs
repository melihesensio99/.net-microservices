namespace Ordering.Domain.Abstractions;

public interface IAggregate<TId> : IEntity<TId>
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}

public interface IAggregate : IAggregate<Guid>
{
}
