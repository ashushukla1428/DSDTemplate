using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.CommonClasses
{
    public class JsTreeModel
    {
        public string data;
        public JsTreeAttribute attr;
        public string state = "open";
        public List<JsTreeModel> children;



        public bool disable { get; set; }
    }

    public class JsTreeAttribute
    {
        public string id;
        public bool selected;
        public string Class;
    }
}