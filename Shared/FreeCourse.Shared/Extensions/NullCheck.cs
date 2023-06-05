using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Extensions
{
    public static class MyExtensions
    {
        public static bool IsNull(this object obj)
        {
            if (obj == null)
                return true;
            
            return false;
        }
        public static bool IsNotNull(this object obj)
        {
            if (obj == null)
                return false;

            return true;
        }
        public static bool IsEmpty(this object obj)
        {
            if (obj == null)
                return true;

            return false;
        }
    }

}
