using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Utilities.Interfaces
{
    public interface IDataResult<TEntity> : IResult where TEntity : class
    {
        TEntity Data { get; }
    }
}
