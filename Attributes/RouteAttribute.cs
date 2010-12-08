using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppVisum.Sys.Attributes
{
    public class RouteAttribute : Attribute
    {
        string route;
        string name = null;

        public RouteAttribute(string route)
        {
            this.route = route;
        }

        public object Defaults { get; set; }
        public object Constraints { get; set; }
        public string Name { set { name = value; } get { throw new NotSupportedException(); } }

        public virtual string GetRoute(string binding)
        {
            if (route[0] == '/')
                return route.Substring(1);
            return binding + '/' + route;
        }

        public virtual string GetName(string defaultName)
        {
            return name ?? defaultName;
        }
    }
}
