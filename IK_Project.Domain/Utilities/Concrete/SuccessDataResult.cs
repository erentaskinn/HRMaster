using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Utilities.Concrete
{
    public class SuccessDataResult<TEntity> : DataResult<TEntity> where TEntity : class
    {
        public SuccessDataResult() :base(default, true){ }
        public SuccessDataResult(string message) :base(default, true, message){ }
        public SuccessDataResult(TEntity data, string message) :base(data, true, message){ }

        // default olunca bu classı oluştururken kullandığın Tentity i baz alıyor, data olunca parametre olarak gönderdiğini baz alıyor.
    }
}
