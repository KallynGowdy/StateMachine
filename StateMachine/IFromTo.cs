/*
    Copyright 2013 Kallyn Gowdy
    
       Licensed under the Apache License, Version 2.0 (the "License");
       you may not use this file except in compliance with the License.
       You may obtain a copy of the License at
    
           http://www.apache.org/licenses/LICENSE-2.0
    
       Unless required by applicable law or agreed to in writing, software
       distributed under the License is distributed on an "AS IS" BASIS,
       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
       See the License for the specific language governing permissions and
       limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KallynGowdy.StateMachine
{
    /// <summary>
    /// Defines an interface for a range of values from the first to the second.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromTo<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the first value in the range.
        /// </summary>
        T First
        {
            get;
        }

        /// <summary>
        /// Gets the second value in the range.
        /// </summary>
        T Second
        {
            get;
        }
        /// <summary>
        /// Determines whether the given value is contained in the range of values that this object stores.
        /// </summary>
        /// <param name="value">The value to compare to the range of values.</param>
        /// <returns>True if the given value is in the range, otherwise false.</returns>
        bool Between(T value);
    }
}
