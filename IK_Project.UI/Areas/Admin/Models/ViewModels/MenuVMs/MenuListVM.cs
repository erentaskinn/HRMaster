﻿using IK_Project.Domain.Enums;

namespace IK_Project.UI.Areas.Admin.Models.ViewModels.MenuVMs
{
    public class MenuListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Period { get; set; }
        public decimal UnitPrice { get; set; }

        public int UserAmount { get; set; }
        public CurrecyUnit Currecy { get; set; }
        public Status Status { get; set; }
    }
}
