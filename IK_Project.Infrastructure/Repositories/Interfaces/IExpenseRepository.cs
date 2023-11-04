using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Interfaces
{
    public interface IExpenseRepository : IAsyncRepository, IAsyncFindableRepository<Expense>, IAsyncInserttableRepository<Expense>, IAsyncOrderableRepository<Expense>, IAsyncUpdatetableRepository<Expense>, ITransactionRepository, IAsyncQueryableRepository<Expense>,IAsyncDeletetableRepository<Expense>, IRepository
	{
    }
}
