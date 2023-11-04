
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess;
using IK_Project.Infrastructure.DataAccess.EntityFramework;
using IK_Project.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Concreates
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
	{
		public AdminRepository(IKProjectDBContext context) : base(context)
		{
		}

		public Task<Admin?> GetByIdentityId(string identityId)
		{
			return _table.FirstOrDefaultAsync(x => x.IdentityId == identityId);
		}
	}
}
