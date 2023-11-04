using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Interfaces
{
    public interface IMenuRepository : IAsyncRepository, IAsyncFindableRepository<Menu>, IAsyncInserttableRepository<Menu>, IAsyncOrderableRepository<Menu>, IAsyncUpdatetableRepository<Menu>, ITransactionRepository, IAsyncQueryableRepository<Menu>,IAsyncDeletetableRepository<Menu>
	{
        Task<IEnumerable<Menu>> DeactiveMenus();
    }
}
