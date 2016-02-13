using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Framework
{
  public class TypeChecking
  {
    public static bool IsNumeric(string s)
    {
      try
      {
        bool blnValid = System.Text.RegularExpressions.Regex.IsMatch(s, "^[0-9.-]*$");
        if (s.IndexOf(".") != s.LastIndexOf("."))
        {
          blnValid = false;
        }
        return blnValid;
      }
      catch
      {
        return false;
      }
    }

    public static int ASC(string s)
    {
      //Return the character value of the given character
      return (int)Encoding.ASCII.GetBytes(s)[0];
    }

    public static string PCase(string strInput)
    {
      int I;
      string CurrentChar, PrevChar;
      string strOutput;

      PrevChar = "";
      strOutput = "";

      for (I = 1; I <= strInput.Length; I++)
      {
        CurrentChar = strInput.Substring(I - 1, 1);

        switch (PrevChar)
        {
          case "":
            strOutput = strOutput + CurrentChar.ToString().ToUpper();
            break;
          case " ":
            strOutput = strOutput + CurrentChar.ToString().ToUpper();
            break;
          case ".":
            strOutput = strOutput + CurrentChar.ToString().ToUpper();
            break;
          case "-":
            strOutput = strOutput + CurrentChar.ToString().ToUpper();
            break;
          case ",":
            strOutput = strOutput + CurrentChar.ToString().ToUpper();
            break;
          case "\"":
            strOutput = strOutput + CurrentChar.ToString().ToUpper();
            break;
          case "'":
            strOutput = strOutput + CurrentChar.ToString().ToUpper();
            break;
          default:
            switch (strOutput.ToUpper().Trim())
            {
              case "MC":
                strOutput = strOutput + CurrentChar.ToString().ToUpper();
                break;
              case "MAC":
                strOutput = strOutput + CurrentChar.ToString();
                break;
              case "O'":
                strOutput = strOutput + CurrentChar.ToString().ToUpper();
                break;
              default:
                strOutput = strOutput + CurrentChar.ToString().ToLower();
                break;
            }
            break;
        }

        PrevChar = CurrentChar;
      }

      return strOutput;
    }
  }
}
