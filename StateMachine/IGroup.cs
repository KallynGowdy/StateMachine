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
namespace KallynGowdy.StateMachine
{
    /// <summary>
    /// Defines a group of states that is able to determine if a given state is inside the group.
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given state is contained in the group.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>True if the given state is contained in the group, False if it is not.</returns>
        bool In(int state);
    }
}
