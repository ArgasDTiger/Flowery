namespace Flowery.Infrastructure.Jobs;

public interface IJobExecutor<in T>
{
    Task Execute(T payload);
}