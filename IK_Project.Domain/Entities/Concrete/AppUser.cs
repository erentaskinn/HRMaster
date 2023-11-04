using IK_Project.Domain.Core.Base;
using IK_Project.Domain.Entities.Abstract;
using IK_Project.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Entities.Concrete
{
    public class AppUser : BaseUser
    {
        public string YourMessage { get; set; }
        public bool? IsCall { get; set; }=false;
        public bool? IsOk { get; set; } = false;
    }
}
