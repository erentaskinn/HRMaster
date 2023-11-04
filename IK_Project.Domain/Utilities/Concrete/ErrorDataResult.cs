using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Utilities.Concrete
{
    public class ErrorDataResult<TEntity> : DataResult<TEntity>where TEntity : class
    {
        public ErrorDataResult() : base(default, false) { }
        public ErrorDataResult(string message) : base(default, false, message) { }
        public ErrorDataResult(TEntity data, string message) : base(data, false, message) { }
    }
}
