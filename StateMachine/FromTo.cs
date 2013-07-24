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
    /// Defines a range of values from the first to the second.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct FromTo<T> : IFromTo<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the first value in the range.
        /// </summary>
        public T First
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last value in the range.
        /// </summary>
        public T Second
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new FromTo object that defines a range of values.
        /// </summary>
        /// <param name="first">The first object in the range.</param>
        /// <param name="second">The last object in the range.</param>
        public FromTo(T first, T second) : this()
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// Determines if the given value is greater-than or equal-to the first element and less-than the second element in this object.
        /// </summary>
        /// <param name="value">The value to compare to the values in this object.</param>
        /// <returns>True if the given value is in the range, otherwise false.</returns>
        public bool Between(T value)
        {
            return First.CompareTo(value) <= 0 && Second.CompareTo(value) > 0;
        }
    }
}