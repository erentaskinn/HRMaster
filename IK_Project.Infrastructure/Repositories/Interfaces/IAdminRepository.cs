using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Interfaces
{
    public interface IAdminRepository : IAsyncRepository, IAsyncFindableRepository<Admin>, IAsyncInserttableRepository<Admin>, IAsyncDeletetableRepository<Admin>, IAsyncUpdatetableRepository<Admin>, ITransactionRepository, IAsyncQueryableRepository<Admin>, IAsyncOrderableRepository<Admin>
	{
		Task<Admin?> GetByIdentityId(string identityId);

	}
}
