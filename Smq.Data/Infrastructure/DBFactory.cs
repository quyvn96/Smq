namespace Smq.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private SmqSolutionDbContext dbContext;

        public SmqSolutionDbContext Init()
        {
            return dbContext ?? (dbContext = new SmqSolutionDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}