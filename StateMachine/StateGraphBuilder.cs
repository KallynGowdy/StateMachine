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
    /// Defines a class that helps with building state graphs.
    /// </summary>
    /// <typeparam name="T">The Type of the value stored in each node.</typeparam>
    /// <typeparam name="TKey">The Type of the value that determines transitions between states.</typeparam>
    public class StateGraphBuilder<TKey, T> : StateMachine.IStateGraphBuilder<StateGraphBuilder<TKey, T>, TKey, T>
    {
        private StateNode<TKey, T> currentNode;

        /// <summary>
        /// Gets (or protected sets) the current node that the builder is at.
        /// </summary>
        public StateNode<TKey, T> CurrentNode
        {
            get
            {
                return currentNode;
            }
            protected set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                currentNode = value;
            }
        }

        private StateGraph<TKey, T> graph;

        /// <summary>
        /// Gets the graph that is being built.
        /// </summary>
        public StateGraph<TKey, T> Graph
        {
            get
            {
                return graph;
            }
        }

        /// <summary>
        /// Creates a new State Graph Builder.
        /// </summary>
        public StateGraphBuilder()
        {
            this.currentNode = new StateNode<TKey, T>();
            this.graph = new StateGraph<TKey, T>(currentNode);
        }

        /// <summary>
        /// Builds the resulting graph.
        /// </summary>
        /// <returns></returns>
        public StateGraph<TKey, T> Build()
        {
            return graph;
        }

        /// <summary>
        /// Adds a transition between the current node and moves to the newly created node.
        /// </summary>
        /// <param name="key">The value that defines the transition value of the created transition.</param>
        /// <param name="value">The value of the newly created node.</param>
        /// <returns></returns>
        public StateGraphBuilder<TKey, T> To(TKey key, T value)
        {
            StateNode<TKey, T> newNode = new StateNode<TKey, T>(value, graph);

            currentNode.AddTransition(key, newNode);

            currentNode = newNode;

            return this;

        }

        /// <summary>
        /// Moves to the next node based on the given transition value.
        /// </summary>
        /// <param name="key">The transition value that determines which node to move to from the current node.</param>
        /// <returns></returns>
        public StateGraphBuilder<TKey, T> To(TKey key)
        {
            currentNode = currentNode[key];
            return this;
        }

        /// <summary>
        /// Creates a transition between the current node and all of the given nodes.
        /// </summary>
        /// <param name="nodes">An enumerable collection of keys relating to a value.</param>
        /// <returns></returns>
        public StateGraphBuilder<TKey, T> To(IEnumerable<KeyValuePair<TKey, T>> nodes)
        {
            foreach (KeyValuePair<TKey, T> keyVal in nodes)
            {
                StateNode<TKey, T> newNode = new StateNode<TKey, T>(keyVal.Value, graph);
                currentNode.AddTransition(keyVal.Key, newNode);
            }
            return this;
        }


        /// <summary>
        /// Creates a transition between the current node and all of the given nodes.
        /// </summary>
        /// <param name="nodes">An enumerable collection of keys relating to a value.</param>
        /// <returns></returns>
        public StateGraphBuilder<TKey, T> To(IEnumerable<KeyValuePair<TKey, StateNode<TKey, T>>> nodes)
        {

            foreach (KeyValuePair<TKey, StateNode<TKey, T>> keyVal in nodes)
            {
                currentNode.AddTransition(keyVal.Key, keyVal.Value);
            }
            return this;

        }

        /// <summary>
        /// Moves to a node that leads to the current node based on the key used to move to the current node.
        /// </summary>
        /// <param name="key">The key that defines the transition between the current node and the node to move back to.</param>
        /// <returns></returns>
        public StateGraphBuilder<TKey, T> Back(TKey key)
        {
            currentNode = currentNode.FindToTransition(key);
            return this;
        }
    }
}
