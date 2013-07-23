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

        public BetweenGroup(string name, IFromTo<int> range)
        {
            this.Name = name;
            this.Range = range;
        }
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

        public bool In(int state)
        {
            return Range.Between(state);
        }

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

        public bool In(int state)
        {
            return States.Contains(state);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
