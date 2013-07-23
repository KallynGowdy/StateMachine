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
    /// Defines an interface for a State Graph Builder.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph builder.</typeparam>
    /// <typeparam name="TKey">The type of values that determine transitions between states.</typeparam>
    /// <typeparam name="T">The type of values to store in the graph.</typeparam>
    interface IStateGraphBuilder<TGraph, TKey, T>
    {
        /// <summary>
        /// Moves the location of the builder back a state based on the given key.
        /// </summary>
        /// <param name="key">The key of the transition that points to the state to move back to.</param>
        /// <returns></returns>
        TGraph Back(TKey key);

        /// <summary>
        /// Builds the graph into a new State Graph object.
        /// </summary>
        /// <returns></returns>
        StateGraph<TKey, T> Build();

        /// <summary>
        /// Gets the current node that the builder is at.
        /// </summary>
        StateNode<TKey, T> CurrentNode
        {
            get;
        }

        /// <summary>
        /// Gets the graph that the builder is using.
        /// </summary>
        StateGraph<TKey, T> Graph
        {
            get;
        }

        /// <summary>
        /// Creates transitions to the given collection of nodes based on the given transitions.
        /// </summary>
        /// <param name="nodes">The collection of transitions (KeyValuePairs) that determine the possible states to move to.</param>
        /// <returns></returns>
        TGraph To(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, StateNode<TKey, T>>> nodes);

        /// <summary>
        /// Creates transitions to the given collection of nodes based on the given transitions.
        /// </summary>
        /// <param name="nodes">The collection of transitions (KeyValuePairs) that determine the possible states to move to.</param>
        /// <returns></returns>
        TGraph To(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, T>> nodes);

        /// <summary>
        /// Creates a transition to a state created from value based on the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        TGraph To(TKey key, T value);

        /// <summary>
        /// Moves to the next node based on the given key.
        /// </summary>
        /// <param name="key">The key that marks the transition to the state to move to.</param>
        /// <returns></returns>
        TGraph To(TKey key);
    }
}
