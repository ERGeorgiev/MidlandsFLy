using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Enum-class describing error codes.
/// </summary>
namespace Database
{
    namespace Enums
    {
        public static class ErrorCode
        {
            public static int WrongColumnName = 207;
            public static int InvalidName = 208;
            public static int TableDoesNotExist = 3701;
            public static int Duplicate = 2627;
            public static int NoConnectionA = 0;
            public static int NoConnectionB = 40;
        }
    }
}