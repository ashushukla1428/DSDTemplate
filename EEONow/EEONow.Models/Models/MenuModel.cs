using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class MenuModel
    {
        public Int32 MenuId { get; set; }
        public String Name { get; set; }
        public Boolean? IsActive { get; set; }
        public Boolean? IsHeader { get; set; }
        public Int32? ParentId { get; set; }

    }
    public class MenuUIModel
    {
        public Int32 MenuId { get; set; }
        public String MenuKey { get; set; }
        public String MenuIcon { get; set; }
        public String MenuUrl { get; set; }
        public String Name { get; set; }
        public Boolean? IsActive { get; set; }
        public Boolean? IsHeader { get; set; }
        public Int32 SortOrder { get; set; }
        public List<MenuUIModel> InnerMenuList { get; set; }
    }


    //public class ChildMenuUIModel
    //{
    //    public Int32 MenuId { get; set; }
    //    public String Name { get; set; }
    //    public String MenuKey { get; set; }
    //    public String MenuIcon { get; set; }
    //    public String MenuUrl { get; set; }
    //    public Boolean? IsActive { get; set; }
    //}

    //public class MenuModel
    //{
    //    public Int32 MenuId { get; set; }
    //    public String Name { get; set; }
    //    public Boolean? IsActive { get; set; }
    //    public Boolean? IsHeader { get; set; }
    //    public Int32? ParentId { get; set; } 
    //}
    //public class MenuUIModel
    //{
    //    public Int32 MenuId { get; set; }
    //    public String MenuKey { get; set; }
    //    public String MenuIcon { get; set; }
    //    public String MenuUrl { get; set; }
    //    public String Name { get; set; }
    //    public Boolean? IsActive { get; set; }
    //    public Boolean? IsHeader { get; set; }
    //    public Int32? ParentId { get; set; }
    //    public List<ChildMenuUIModel> _SubMenuLst { get; set; }
    //}
    //public class ChildMenuUIModel
    //{
    //    public Int32 MenuId { get; set; }
    //    public String Name { get; set; }
    //    public String MenuKey { get; set; }
    //    public String MenuIcon { get; set; }
    //    public String MenuUrl { get; set; }
    //    public Boolean? IsActive { get; set; } 
    //}
}
