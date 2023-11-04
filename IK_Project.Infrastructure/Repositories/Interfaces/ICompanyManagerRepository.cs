using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Interfaces
{
    public interface ICompanyManagerRepository : IAsyncRepository, IAsyncFindableRepository<CompanyManager>, IAsyncInserttableRepository<CompanyManager>, IAsyncOrderableRepository<CompanyManager>, IAsyncUpdatetableRepository<CompanyManager>, ITransactionRepository, IAsyncQueryableRepository<CompanyManager>, IAsyncDeletetableRepository<CompanyManager>
	{

    }
}
