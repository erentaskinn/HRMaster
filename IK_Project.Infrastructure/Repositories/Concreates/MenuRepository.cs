﻿using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess;
using IK_Project.Infrastructure.DataAccess.EntityFramework;
using IK_Project.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Repositories.Concreates
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IKProjectDBContext dBContext) : base(dBContext)
        {

        }


        public async Task<IEnumerable<Menu>> DeactiveMenus()
        {
            return await GetDefault(x => x.IsActive == true);
        }
    }
}
