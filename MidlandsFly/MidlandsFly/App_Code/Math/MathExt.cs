using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MathExtensions
/// </summary>
public static class MathExt
{
    public static string IntToFixedString(int number, byte length)
    {
        string output = number.ToString();
        while (output.Length < length)
        {
            output = "0" + output;
        }
        return output;
    }
}