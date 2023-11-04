using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess;
using IK_Project.Infrastructure.DataAccess.EntityFramework;
using IK_Project.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Concreates
{
    public class DepartmantManagerRepository:BaseRepository<DepartmantManager>,IDepartmanManagerRepository
    {
        public DepartmantManagerRepository(IKProjectDBContext DbContext):base(DbContext) { }
                  
        
    }
}
