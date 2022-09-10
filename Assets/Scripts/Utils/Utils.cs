using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;

public class Utils : MonoBehaviour
{
    public static T ConvertStringToEnum<T>(string stringValue)
    {
        T enumValue;
        if (Enum.IsDefined(typeof(T), stringValue))
        {
            enumValue = (T)Enum.Parse(typeof(T), stringValue);
            return enumValue;
        }
        else
        {
            throw new NotImplementedException(String.Format("{0} is not defined in {1}", stringValue, typeof(T).ToString()));
        }
    }

#if UNITY_EDITOR
    public static void ClearLog()
    {
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
#endif

    /// <summary>
    /// Call this when use Invoke so that functions are still refered to
    /// </summary>
    public static string GetMethodName(UnityEngine.Events.UnityAction action)
    {
        return action.Method.Name;
    }
    public static string GetCoroutinename(IEnumerator coroutine)
    {
        // coroutine name has format of
        // Login+<CountDownSendOTP>d__33
        string subString = coroutine.ToString().Substring(coroutine.ToString().IndexOf('<') + 1,
                                                        coroutine.ToString().IndexOf('>') - coroutine.ToString().IndexOf('<') - 1).Trim();
        return subString;
    }
    #region Cryptography
    const string KEY = "cscmobi";
    public static string XOROperator(string input)
    {
        char[] output = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
            output[i] = (char)(input[i] ^ KEY[i % KEY.Length]);
        return new string(output);
    }
    public static string GenerateSHA256NonceFromRawNonce(string rawNonce)
    {
        var sha = new SHA256Managed();
        var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
        var hash = sha.ComputeHash(utf8RawNonce);

        var result = string.Empty;
        for (var i = 0; i < hash.Length; i++)
        {
            result += hash[i].ToString("x2");
        }

        return result;
    }
    public static string GenerateRandomString(int length)
    {
        if (length <= 0)
        {
            throw new Exception("Expected nonce to have positive length");
        }
        const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
        var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
        var result = string.Empty;
        var remainingLength = length;
        var randomNumberHolder = new byte[1];
        while (remainingLength > 0)
        {
            var randomNumbers = new List<int>(16);
            for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++)
            {
                cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
                randomNumbers.Add(randomNumberHolder[0]);
            }
            for (var randomNumberIndex = 0; randomNumberIndex < randomNumbers.Count; randomNumberIndex++)
            {
                if (remainingLength == 0)
                    break;
                var randomNumber = randomNumbers[randomNumberIndex];
                if (randomNumber < charset.Length)
                {
                    result += charset[randomNumber];
                    remainingLength--;
                }
            }
        }
        return result;
    }
    #endregion
    #region Static Time Helper
    public static double ConvertToUnixTime(DateTime time)
    {
        DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (time - epoch).TotalSeconds;
    }
    public static DateTime ConvertFromUnixTime(double timeStamp)
    {
        DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        DateTime time = epoch.AddSeconds(timeStamp);
        return time;
    }
    public static int GetDays(float totalSeconds)
    {
        return (int)(totalSeconds / 86400f);
    }
    public static int GetHoursInDay(float totalSeconds)
    {
        return (int)((totalSeconds / 3600f)) % 24;
    }
    public static int GetHours(float totalSeconds)
    {
        return (int)(totalSeconds / 3600f);
    }
    public static int GetMinutes(float totalSeconds)
    {
        return (int)(totalSeconds / 60) % 60;
    }
    public static int GetSeconds(float totalSeconds)
    {
        return (int)(totalSeconds % 60);
    }
    public static int GetNextDayUnixTime(float unixTimeNow)
    {
        DateTime dateTimeNow = ConvertFromUnixTime(unixTimeNow);
        DateTime nextDay = (dateTimeNow - dateTimeNow.TimeOfDay).AddDays(1);
        return (int)ConvertToUnixTime(nextDay);
    }

    public static string FormatTime(int timeMinute)
    {
        int hour = timeMinute / 60;
        int minute = timeMinute % 60;
        if (hour == 0)
        {
            return string.Format("{0:00}:{1:00}", minute, 0);
        }
        return string.Format("{0:0}:{1:00}:{2:00}", hour, minute, 0);
    }
    public static string FormatTimeSecond(int timeSecond)
    {
        int hour = timeSecond / 3600;
        int minute = (timeSecond / 60) % 60;
        int second = timeSecond % 60;
        if (hour == 0)
        {
            return string.Format("{0:00}:{1:00}", minute, second);
        }
        return string.Format("{0:0}:{1:00}:{2:00}", hour, minute, second);
    }
    #endregion
}
