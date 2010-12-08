using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppVisum.Sys.Utilities;

namespace AppVisum.Sys
{
    public static class Utils
    {
        public static object MergeObjects(params object[] args)
        {
            ObjectMerger merger = new ObjectMerger();
            merger.AddRange(args.Where(arg => arg != null));
            return merger.Merge();
        }
    }
}
