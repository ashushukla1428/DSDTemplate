using System.Collections.Generic;

namespace dsdProjectTemplate.ViewModel.Menu
{
    public class UserMenuList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MenuController { get; set; }
        public string MenuAction { get; set; }
        public string MenuIcon { get; set; }
        public bool IsHeader { get; set; }
        public int DisplayOrder { get; set; }
        public string AreaName { get; set; }
        public List<UserSubMenuList> SubMenus { get; set; }
    }
    public class UserSubMenuList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MenuController { get; set; }
        public string MenuAction { get; set; }
        public string MenuIcon { get; set; }
        public int DisplayOrder { get; set; }
        public string AreaName { get; set; }
        public int MenuHeaderID { get; set; }

    }
}
