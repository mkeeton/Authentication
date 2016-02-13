using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Authentication.Framework
{
  public class NullHandlers
  {
    public static Guid NGUID(object ObjValue)
    {
      Guid ReturnID;
      if (NES(ObjValue) == "")
      {
        ReturnID = new Guid();
      }
      else
      {
        try
        {
          ReturnID = new Guid(NES(ObjValue));
        }
        catch (Exception ex)
        {
          ReturnID = new Guid();
        }
      }
      return ReturnID;
    }

    public static object NGUID(object ObjValue, bool ForDB)
    {
      Guid GTemp = NGUID(ObjValue);
      if (GTemp == Guid.Empty)
      {
        if (ForDB == true)
        {
          return DBNull.Value;
        }
        else
        {
          return GTemp;
        }
      }
      else
      {
        return GTemp;
      }
    }

    public static string NES(object ObjValue)
    {
      if (ObjValue == null)
      {
        return "";
      }
      else if (ObjValue == DBNull.Value)
      {
        return "";
      }
      else
      {
        return ObjValue.ToString();
      }
    }

    public static DateTime NTODAY(object ObjValue)
    {
      try
      {
        return System.DateTime.Parse(NES(ObjValue));
      }
      catch (Exception ex)
      {
        return System.DateTime.Now;
      }
    }

    public static object NDATE(object ObjValue)
    {
      return NDATE(ObjValue, false);
    }

    public static object NDATE(object ObjValue, bool blnForDB)
    {
      try
      {
        return System.DateTime.Parse(NES(ObjValue));
      }
      catch (Exception ex)
      {
        if (blnForDB == true)
        {
          return DBNull.Value;
        }
        else
        {
          return null;
        }
      }
    }

    public static double NDOUB(object ObjValue)
    {
      try
      {
        if (Convert.IsDBNull(ObjValue) == false)
        {
          if (TypeChecking.IsNumeric(ObjValue.ToString()) == true)
          {
            return Convert.ToDouble(ObjValue);
          }
          else
          {
            return 0;
          }
        }
        else
        {
          return 0;
        }
      }
      catch (Exception ex)
      {
        return 0;
      }
    }

    public static int NI(object ObjValue)
    {
      return NI(ObjValue, 0);
    }

    public static int NI(object ObjValue, int intDefault)
    {
      try
      {
        if (!(ObjValue == null))
        {
          if (Convert.IsDBNull(ObjValue) == false)
          {
            if (TypeChecking.IsNumeric(ObjValue.ToString()) == true)
            {
              return Convert.ToInt32(ObjValue);
            }
            else
            {
              return intDefault;
            }
          }
          else
          {
            return intDefault;
          }
        }
        else
        {
          return intDefault;
        }
      }
      catch (Exception ex)
      {
        return intDefault;
      }
    }

    public static Int64 NLONG(object ObjValue)
    {
      try
      {
        if (Convert.IsDBNull(ObjValue) == false)
        {
          if (ObjValue == null)
          {
            return 0;
          }
          else
          {
            if (TypeChecking.IsNumeric(ObjValue.ToString()) == true)
            {
              return Convert.ToInt64(ObjValue);
            }
            else
            {
              return 0;
            }
          }
        }
        else
        {
          return 0;
        }
      }
      catch (Exception ex)
      {
        return 0;
      }
    }

    public static bool NBOOL(object ObjValue)
    {
      try
      {
        if (Convert.IsDBNull(ObjValue) == false)
        {
          return Convert.ToBoolean(ObjValue);
        }
        else
        {
          return false;
        }
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static object NDBNULL(object ObjValue)
    {
      if (ObjValue == null)
      {
        return DBNull.Value;
      }
      else if (ObjValue == DBNull.Value)
      {
        return null;
      }
      else
      {
        return ObjValue;
      }
    }

    public static string SQLDate(DateTime ObjValue, bool WithTime)
    {
      if (WithTime == true)
      {
        return ObjValue.Month.ToString() + "/" + ObjValue.Day.ToString() + "/" + ObjValue.Year.ToString() + " " + ObjValue.Hour.ToString() + ":" + ObjValue.Minute.ToString() + ":" + ObjValue.Second.ToString();
      }
      else
      {
        return ObjValue.Month.ToString() + "/" + ObjValue.Day.ToString() + "/" + ObjValue.Year.ToString();
      }
    }

    public static string UKDate(DateTime ObjValue, bool WithTime)
    {
      if (WithTime == true)
      {
        return ObjValue.Day.ToString() + "/" + ObjValue.Month.ToString() + "/" + ObjValue.Year.ToString() + " " + ObjValue.Hour.ToString() + ":" + ObjValue.Minute.ToString() + ":" + ObjValue.Second.ToString();
      }
      else
      {
        return ObjValue.Day.ToString() + "/" + ObjValue.Month.ToString() + "/" + ObjValue.Year.ToString();
      }
    }

    public static string StripTagsRegex(string source)
    {
      return Regex.Replace(source, "<.*?>", string.Empty);
    }

    /// <summary>
    /// Compiled regular expression for performance.
    /// </summary>
    static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

    /// <summary>
    /// Remove HTML from string with compiled Regex.
    /// </summary>
    public static string StripTagsRegexCompiled(string source)
    {
      return _htmlRegex.Replace(source, string.Empty);
    }

    /// <summary>
    /// Remove HTML tags from string using char array.
    /// </summary>
    public static string StripTagsCharArray(string source)
    {
      char[] array = new char[source.Length];
      int arrayIndex = 0;
      bool inside = false;

      for (int i = 0; i < source.Length; i++)
      {
        char let = source[i];
        if (let == '<')
        {
          inside = true;
          continue;
        }
        if (let == '>')
        {
          inside = false;
          continue;
        }
        if (!inside)
        {
          array[arrayIndex] = let;
          arrayIndex++;
        }
      }
      return new string(array, 0, arrayIndex);
    }
  }
}
