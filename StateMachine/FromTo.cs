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
    /// Defines a range of value from the first to the second.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct FromTo<T> : IFromTo<T> where T : IComparable<T>
    {
        public T First
        {
            get;
            private set;
        }

        public T Second
        {
            get;
            private set;
        }

        public FromTo(T first, T second) : this()
        {
            this.First = first;
            this.Second = second;
        }

        public bool Between(T value)
        {
            return First.CompareTo(value) <= 0 && Second.CompareTo(value) > 0;
        }
    }
}