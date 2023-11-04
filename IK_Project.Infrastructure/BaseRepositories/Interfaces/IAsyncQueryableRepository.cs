using IK_Project.Domain.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncQueryableRepository<TEntity>: IAsyncRepository where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking=true);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking=true);
        Task<IEnumerable<TEntity>> GetDefault(Expression<Func<TEntity, bool>> expression, bool tracking = true);
	}
}
