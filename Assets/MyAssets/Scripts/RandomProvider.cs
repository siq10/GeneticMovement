using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class RandomProvider
{
    private static RNGCryptoServiceProvider wholepart = new RNGCryptoServiceProvider();
    private static RNGCryptoServiceProvider fractionalpart = new RNGCryptoServiceProvider();
    private static RNGCryptoServiceProvider signpart = new RNGCryptoServiceProvider();
    private static RNGCryptoServiceProvider geneticopspart = new RNGCryptoServiceProvider();


    public static float GetRandomFloat(float minInclusive, float maxInclusive)
    {
        string sign = "";
        if(!GetSign())
        {
            sign = "-";
        }
        string x = sign + GetInt((int)maxInclusive) + GetFraction(GetFractionalDigitsCount());
        return Convert.ToSingle(x);
    }

    //max 255
    private static int GetInt(int maxInclusive)
    {
        byte[] arr = new byte[1];
        wholepart.GetBytes(arr);
        return arr[0] % (maxInclusive);
    }

    private static bool GetSign()
    {
        byte[] arr = new byte[1];
        signpart.GetBytes(arr);
        return Convert.ToBoolean(arr[0] % 2);
    }
    private static byte GetFractionalDigitsCount()
    {
        byte[] arr = new byte[1];
        signpart.GetBytes(arr);
        return (byte)(arr[0] % 8);
    }

    private static string GetFraction(byte count)
    {
        StringBuilder s = new StringBuilder(".");
        byte[] arr = new byte[count];
        byte remainingBytesNeeded = count;
        while (remainingBytesNeeded != 0)
        {
            fractionalpart.GetBytes(arr);
            for (int i = 0; i < count; i++)
            {
                if (IsFairRandom(arr[i], 10))
                {
                    s.Append(arr[i]);
                    if (--remainingBytesNeeded == 0)
                        break;
                }
            }
        }
        return s.ToString();
    }
    private static bool IsFairRandom(byte number, byte rangeofvalues)
    {
        int fullSetsOfValues = Byte.MaxValue / rangeofvalues;
        return number < rangeofvalues * fullSetsOfValues;
    }


}