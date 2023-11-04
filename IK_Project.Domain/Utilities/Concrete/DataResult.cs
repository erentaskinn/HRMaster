using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Utilities.Concrete
{
    public class DataResult<TEntity> : Result, IDataResult<TEntity> where TEntity : class
    {
        public TEntity Data { get; }

        public DataResult(TEntity data, bool isSuccess) : base(isSuccess)
        {
            Data = data;
        }
        public DataResult(TEntity data, bool isSuccess, string message): base(isSuccess, message)
        {
            Data = data;
        }
    }
}
