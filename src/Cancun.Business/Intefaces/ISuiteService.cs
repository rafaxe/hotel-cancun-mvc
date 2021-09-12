using System;
using System.Threading.Tasks;
using Cancun.Business.Models;

namespace Cancun.Business.Intefaces
{
    public interface ISuiteService : IDisposable
    {
        Task Add(Suite suite);
        Task Update(Suite suite);
        Task Remove(Guid id);
    }
}