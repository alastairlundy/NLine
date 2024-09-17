/*
    BasisBox - NLine Library
    Copyright (C) 2024 Alastair Lundy

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlastairLundy.Extensions.System;

namespace NLine.Library;

public static class LineNumberer
{
    internal static int CalculateLineNumber(int currentIndex, int lineIncrementor, int initialLineNumber)
    {
        return currentIndex == 0 ? initialLineNumber : initialLineNumber * ((currentIndex + 1) * lineIncrementor);
    }

    internal static string AddColumns(int columnNumber)
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        if (columnNumber > 0)
        {
            for (int column = 1; column <= columnNumber; column++)
            {
                stringBuilder.Append(' ');
            }

            return stringBuilder.ToString();
        }
        // ReSharper disable once RedundantIfElseBlock
        else
        {
            return string.Empty;
        }
    }

    internal static string AddLeadingZeroes(int lineNumberDigits)
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        int zeroesToAdd = 5 - lineNumberDigits;

        if (zeroesToAdd > 0)
        {
            for (int zero = 0; zero < zeroesToAdd; zero++)
            {
                stringBuilder.Append('0');
            }

            return stringBuilder.ToString();
        }

        return string.Empty;
    }

    internal static bool NextXLinesIsEmpty(int numberOfLines, int currentIndex, string[] lines)
    {
        if (lines[currentIndex].Equals(string.Empty))
        {
            bool[] checkedLines = new bool[numberOfLines];
            
            bool linesChecked = false;
            int internalIndex = 1;
            
            while (!linesChecked)
            {
                int lineToBeChecked = currentIndex + internalIndex;
                // ReSharper disable once ArrangeRedundantParentheses
                if ((lines.Length > lineToBeChecked) && lineToBeChecked <= numberOfLines)
                {
                    checkedLines[lineToBeChecked] = lines[lineToBeChecked] == string.Empty;
                }

                if (lineToBeChecked > numberOfLines)
                {
                    linesChecked = true;
                }
            }

            return checkedLines.IsAllTrue();
        }

        return false;
    }

    internal static string AddTabSpacesIfNeeded(bool addTabSpaces, string line)
    {
        if (addTabSpaces)
        {
            return $"\t {line}";
        }
        else
        {
            return line;
        }
    }

    internal static string ConstructLine(int lineNumber, int columnNumber, string line, string lineNumberAppendedText, bool addTabSpaces, bool addLeadingZeroes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        stringBuilder.Append(AddColumns(columnNumber));

        if (addLeadingZeroes)
        {
            stringBuilder.Append(AddLeadingZeroes(lineNumber.ToString().Length));
        }
            
        stringBuilder.Append(lineNumber);
        stringBuilder.Append(lineNumberAppendedText);

        stringBuilder.Append(AddTabSpacesIfNeeded(addTabSpaces, line));

        return stringBuilder.ToString();
    }

    /// <summary>
    /// The simple line numbering method which uses default settings to add line numbers to an IEnumerable of type String.
    /// </summary>
    /// <param name="lines">The lines to be numbered.</param>
    /// <param name="lineNumberAppendedText">The string to follow the line number. If you want nothing to be appended use string.Empty</param>
    /// <returns>a new IEnumerable containing the contents of the provided IEnumerable with line numbering added to it.</returns>
    public static IEnumerable<string> AddLineNumbers(IEnumerable<string> lines, string lineNumberAppendedText)
    {
        return AddLineNumbers(lines, 1, 1, lineNumberAppendedText, true, 0, 4, false, false, null);
    }
    
    /// <summary>
    /// The advanced line numbering method with many customization options.
    /// </summary>
    /// <param name="lines">The lines to be numbered.</param>
    /// <param name="lineIncrementor">The amount to increase each line number by.</param>
    /// <param name="initialLineNumber">The initial number to use as a line number.</param>
    /// <param name="lineNumberAppendedText">The text to append to the line number. If you want nothing to be appended use string.Empty</param>
    /// <param name="assignEmptyLinesANumber">Whether to assign a line number to empty lines.</param>
    /// <param name="numberOfEmptyLinesToGroupTogether">The number of consecutive empty lines to be given a line number.</param>
    /// <param name="columnNumber">The column to use for the line number.</param>
    /// <param name="tabSpaceAfterLineNumber">The amount of tab spaces after the line number and before the line contents.</param>
    /// <param name="addLeadingZeroes">Whether to add leading zeroes to the line number.</param>
    /// <param name="listNumbersWithString">An optional parameter to allow for only numbering lines with a specified string.</param>
    /// <returns></returns>
    public static IEnumerable<string> AddLineNumbers(IEnumerable<string> lines, int lineIncrementor, int initialLineNumber, string lineNumberAppendedText, bool assignEmptyLinesANumber, int numberOfEmptyLinesToGroupTogether, int columnNumber, bool tabSpaceAfterLineNumber, bool addLeadingZeroes, string? listNumbersWithString = null)
    {
        List<string> list = new List<string>();
        
        string[] enumerable = lines as string[] ?? lines.ToArray();
        
        for(int index = 0; index < enumerable.Length; index++)
        {
            string line = enumerable[index];

            int lineNumber = CalculateLineNumber(index, lineIncrementor, initialLineNumber);
            
            if ((!assignEmptyLinesANumber && !line.Equals(string.Empty) && listNumbersWithString == null) ||
                (listNumbersWithString != null && line.Contains(listNumbersWithString)) ||
                (assignEmptyLinesANumber && line.Equals(string.Empty)))
            {
                if (line.Equals(string.Empty) && NextXLinesIsEmpty(numberOfEmptyLinesToGroupTogether, index, enumerable) && assignEmptyLinesANumber)
                {
                    if (numberOfEmptyLinesToGroupTogether > 1)
                    {
                        for (int emptyLine = 0; emptyLine < numberOfEmptyLinesToGroupTogether % 2; emptyLine++)
                        {
                            list.Add(string.Empty);
                        }
                   
                        list.Add(ConstructLine(lineNumber, columnNumber, line, lineNumberAppendedText,
                            tabSpaceAfterLineNumber, addLeadingZeroes));
                   
                        for (int emptyLine = 0; emptyLine < numberOfEmptyLinesToGroupTogether % 2; emptyLine++)
                        {
                            list.Add(string.Empty);
                        }
                        
                        index += numberOfEmptyLinesToGroupTogether - 1;
                    }
                    else
                    {
                        list.Add(ConstructLine(lineNumber, columnNumber, line, lineNumberAppendedText, tabSpaceAfterLineNumber, addLeadingZeroes));
                    }
                }
                
                list.Add(ConstructLine(lineNumber, columnNumber, line, lineNumberAppendedText, tabSpaceAfterLineNumber, addLeadingZeroes));
            }
        }

        return list.ToArray();
    }
}