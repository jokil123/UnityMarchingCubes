using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static string ReverseString(string s)
    {
        char[] charArray = s.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }
}