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
    /// Defines a group of states between certian numbered states.
    /// </summary>
    public class BetweenGroup : StateMachine.IGroup
    {
        /// <summary>
        /// Creates a new group that encapsulates a given range of states.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="range"></param>
        public BetweenGroup(string name, IFromTo<int> range)
        {
            this.Name = name;
            this.Range = range;
        }
        
        /// <summary>
        /// Gets the range of states that this group encloses.
        /// </summary>
        public IFromTo<int> Range
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given state is contained in this group.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool In(int state)
        {
            return Range.Between(state);
        }

        /// <summary>
        /// Gets the name of this group.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Defines a group of states between certian numbered states.
    /// </summary>
    public class NumberedGroup : StateMachine.IGroup
    {
        /// <summary>
        /// Creates a new group with the given name and specific states that the group should contain.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="states"></param>
        public NumberedGroup(string name, params int[] states)
        {
            if(name != null)
            {
                throw new ArgumentNullException("name");
            }
            this.Name = name;
            States = new HashSet<int>(states);
        }

        /// <summary>
        /// Gets the collection of states that this group contains.
        /// </summary>
        public HashSet<int> States
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given state is contained in this group.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool In(int state)
        {
            return States.Contains(state);
        }

        /// <summary>
        /// Gets the name of this group.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
