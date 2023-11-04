using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmantRepository : IAsyncRepository, IAsyncFindableRepository<Departmant>, IAsyncInserttableRepository<Departmant>, IAsyncOrderableRepository<Departmant>, IAsyncUpdatetableRepository<Departmant>, ITransactionRepository, IAsyncQueryableRepository<Departmant>,IAsyncDeletetableRepository<Departmant>
	{

    }
}
