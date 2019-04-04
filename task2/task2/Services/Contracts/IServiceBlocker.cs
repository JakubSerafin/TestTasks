namespace task2.Models.Services.Contracts
{
    public interface IServiceBlocker
    {
        bool WaitCanProcess();
    }
}
