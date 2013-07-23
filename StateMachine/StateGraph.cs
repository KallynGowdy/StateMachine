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
    /// Provides a graph that relates states to other states based on transitions.
    /// This provides the most deterministic automa for a CFG.
    /// </summary>
    public class StateGraph<TKey, T>
    {
        /// <summary>
        /// Gets or sets the root node of the graph.
        /// This is the first node in the graph.
        /// </summary>
        public StateNode<TKey, T> Root
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new empty graph.
        /// </summary>
        public StateGraph()
        {
            this.Root = null;
        }

        /// <summary>
        /// Creates a new StateGraph with the given rootNode as the Root.
        /// </summary>
        /// <param name="rootNode"></param>
        public StateGraph(StateNode<TKey, T> rootNode)
        {
            this.Root = rootNode;
        }

        /// <summary>
        /// Determines if the graph contains the given node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Contains(StateNode<TKey, T> node)
        {
            return Root.ContainsFromTransition(node);
        }

        /// <summary>
        /// Determines if the graph contains a node that matches the given condition.
        /// </summary>
        /// <param name="condition">The condition that determines if a node is a match.</param>
        /// <param name="maxLoop">The maximum number of nodes to travel to, this prevents infinite loops.</param>
        /// <returns></returns>
        public bool Contains(Predicate<StateNode<TKey, T>> condition)
        {
            return Root.ContainsFromTransition(condition);
        }

        /// <summary>
        /// Gets the first node from the graph that node matches the given condition.
        /// </summary>
        /// <param name="condition">The condition that determines if a node is a match.</param>
        /// <param name="maxLoop">The maximum number of nodes to travel to, this prevents infinite loops.</param>
        /// <returns></returns>
        public StateNode<TKey, T> FindExistingNode(Predicate<StateNode<TKey, T>> condition)
        {
            return Root.GetFromTransition(condition);
        }

        /// <summary>
        /// Gets the depth first traversal of the graph.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StateNode<TKey, T>> GetDepthFirstTraversal()
        {
            return Root.GetDepthFirstTraversal();
        }



        /// <summary>
        /// Gets the breadth first traversal of the graph.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StateNode<TKey, T>> GetBreadthFirstTraversal()
        {
            return Root.GetBreadthFirstTraversal();
        }

    }
}
