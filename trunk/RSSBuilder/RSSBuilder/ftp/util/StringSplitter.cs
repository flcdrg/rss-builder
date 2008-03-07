// edtFTPnet
// 
// Copyright (C) 2006 Enterprise Distributed Technologies Ltd
// 
// www.enterprisedt.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// Bug fixes, suggestions and comments should posted on 
// http://www.enterprisedt.com/forums/index.php
// 
// Change Log:
// 
// $Log: StringSplitter.cs,v $
// Revision 1.1  2006/06/19 12:35:35  bruceb
// convenience class
//
//
//

using System;
using System.Collections;
using System.Text;

namespace EnterpriseDT.Util
{    
	/// <summary>
	/// Useful for splitting strings into fields. A bit cleaner
	/// than a regex for what we want to do
	/// </summary>
	public class StringSplitter
	{

        /// <summary>
        /// Defines interface for checking a char is a token
        /// </summary>
        public interface ITokenChecker 
        {
            bool IsToken(char ch);
        }

        /// <summary>
        /// Defines interface for checking a char is whitespace
        /// </summary>
        public class WhitespaceTokenChecker : ITokenChecker 
        {
            public bool IsToken(char ch) { return Char.IsWhiteSpace(ch); }
        }

        /// <summary>
        /// Defines interface for checking a char matches a supplied token char
        /// </summary>
        public class CharTokenChecker : ITokenChecker 
        {
            private char token;
            public CharTokenChecker(char token) { this.token = token; }
            public bool IsToken(char ch) { return ch == token; }
        }

        /// <summary>
        /// Splits string consisting of fields separated by
        /// whitespace into an array of strings
        /// </summary>
        /// <param name="str">
        /// string to split
        /// </param>   
        public static string[] Split(string str) 
        {
            WhitespaceTokenChecker tc = new WhitespaceTokenChecker();
            return Split(str, tc);
        }

        /// <summary>
        /// Splits string consisting of fields separated by
        /// a char separator into an array of strings
        /// </summary>
        /// <param name="str">string to split</param>
        /// <param name="sep">separator char</param>
        /// <returns></returns>
        public static string[] Split(string str, char sep) 
        {
            CharTokenChecker tc = new CharTokenChecker(sep);
            return Split(str, tc);
        }


        /// <summary>
        /// Splits string consisting of fields separated by
        /// a char separator into an array of strings
        /// </summary>
        /// <param name="str">string to split</param>
        /// <param name="sep">separator char</param>
        /// <returns></returns>
        public static string[] Split(string str, ITokenChecker checker) 
        {
            ArrayList fields = new ArrayList();
            StringBuilder field = new StringBuilder();
            for (int i = 0; i < str.Length; i++) 
            {
                char ch = str[i];
                if (!checker.IsToken(ch))
                    field.Append(ch);
                else 
                {
                    if (field.Length > 0) 
                    {
                        fields.Add(field.ToString());
                        field.Length = 0;
                    }
                }
            }
            // pick up last field
            if (field.Length > 0) 
            {
                fields.Add(field.ToString());
            }
            string[] result = (string[])fields.ToArray(typeof(string));
            return result;
        }
	}
}
