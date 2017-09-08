namespace Smq.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}