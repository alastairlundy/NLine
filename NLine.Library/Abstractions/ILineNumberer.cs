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

namespace NLine.Library.Abstractions
{
    public interface ILineNumberer
    { 
        int CalculateNextLineNumber(int currentIndex, int lineIncrementor, int initialLineNumber);
        
        IEnumerable<string> AddLineNumbers(IEnumerable<string> lines, string lineNumberAppendedText);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        #nullable enable
        
        IEnumerable<string> AddLineNumbers(IEnumerable<string> lines, int lineIncrementor, int initialLineNumber,
            string lineNumberAppendedText, bool assignEmptyLinesANumber, int numberOfEmptyLinesToGroupTogether,
            int columnNumber, bool tabSpaceAfterLineNumber, bool addLeadingZeroes, string? listNumbersWithString = null);
#else
        IEnumerable<string> AddLineNumbers(IEnumerable<string> lines, int lineIncrementor, int initialLineNumber,
            string lineNumberAppendedText, bool assignEmptyLinesANumber, int numberOfEmptyLinesToGroupTogether,
            int columnNumber, bool tabSpaceAfterLineNumber, bool addLeadingZeroes, string listNumbersWithString = null);
#endif
    }
}