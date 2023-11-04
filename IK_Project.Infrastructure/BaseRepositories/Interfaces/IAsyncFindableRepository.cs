using IK_Project.Domain.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncFindableRepository<TEntity>: IAsyncRepository where TEntity : BaseEntity
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression=null);
        Task<TEntity?> GetByIdAsync(Guid id, bool tracing = true);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, bool tracing = true);
        Task<List<TEntity?>> GetByIdsAsync(List<Guid> ids, bool tracing = true);
        Task<TEntity?> GetByIdAsyncNull(Guid? id, bool tracing = true);

	}
}
