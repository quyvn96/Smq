using System;

namespace Smq.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        SmqSolutionDbContext Init();
    }
}