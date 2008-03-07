using System;

namespace RSSBuilder
{
	/// <summary>
	/// Summary description for EncryptString.
	/// </summary>
	public class Encryption
	{
      public static string Encrypt(string inputStr)
      {	
         int len = inputStr.Length;
	
         int sum = 10;
	
         char[] encryptedChars = new char[len];
         for (int i=0; i<len; i++) 
         {
            encryptedChars[i] = (char)((int)inputStr[i] + sum);
         }
	
         char[] newNameStr = new char[len];
         for (int i=0; i<len; i++) 
         { // reverse the name
            newNameStr[i] = encryptedChars[len-1-i];
         }

         for (int i=0; i<len; i++) 
         { // reverse the name
            encryptedChars[i] = newNameStr[i];
         }
	
         return String.Intern(new String(encryptedChars));
      }		

      public static string Decrypt(string inputStr)
      {	
         int len = inputStr.Length;
         char[] decryptedChars = new char[len];
         for (int i=0; i<len; i++) 
         {
            decryptedChars[i] = (char)((int)inputStr[i] - 10);
         }	

         char[] newNameStr = new char[len];
         for (int i=0; i<len; i++) 
         { // reverse the name
            newNameStr[i] = decryptedChars[len-1-i];
         }

         for (int i=0; i<len; i++) 
         { // reverse the name
            decryptedChars[i] = newNameStr[i];
         }

         return String.Intern(new String(decryptedChars));
      }

      public static string ReshuffleString(string strValue)
      {
         return Encryption.ShuffleString(strValue);
      }

      public static string ShuffleString(string strValue)
      {
         int i,j,k;
         string strNewStringValue="";
         char [] chrValue = strValue.ToCharArray();

         for ( i=0; i<=chrValue.Length-1; i++)
         {
            // take the next character in the string
            j = (int)chrValue[i];

            // find out the character code
            k = (int)j;

            if (k >= 97 && k <= 109)
            {
               // a ... m inclusive become n ... z
               k = k + 13;
            }
            else if( k >= 110 && k <= 122 )
            {
               // n ... z inclusive become a ... m
               k = k - 13;
            }
            else if (k >= 65 && k <= 77)
            {
               // A ... m inclusive become n ... z
               k = k + 13 ;
            }
            else if( k >= 78 && k <= 90 )
            {
               // N ... Z inclusive become A ... M
               k = k - 13;
            }
			
            //add the current character to the string returned by the function
            strNewStringValue = strNewStringValue +(char)k;
         }

         return strNewStringValue ;				
      }
	}
}
