using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Menu.MenuAreas
{
    public class MenuAreasService : IMenuAreasService
    {
        public List<SelectListItem> GetAreas()
        {
            var _listData = new List<SelectListItem>();
            _listData.Add(new SelectListItem { Text = "NA", Value = "1" });
            _listData.Add(new SelectListItem { Text = "Masters", Value = "1" });
            _listData.Add(new SelectListItem { Text = "Reports", Value = "2" });

            return _listData;
        }
    }
}
