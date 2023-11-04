using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Interfaces
{
    public interface IAdvanceRepository : IAsyncRepository, IAsyncFindableRepository<Advance>, IAsyncInserttableRepository<Advance>, IAsyncOrderableRepository<Advance>, IAsyncUpdatetableRepository<Advance>, ITransactionRepository, IAsyncQueryableRepository<Advance>, IAsyncDeletetableRepository<Advance>, IRepository
	{
    }
}
