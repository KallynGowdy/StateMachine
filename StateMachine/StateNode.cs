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
    /// Defines a state that leads to other states based on "transitions" determined by "Keys".
    /// </summary>
    /// <typeparam name="T">The value that the Node should store.</typeparam>
    /// <typeparam name="TKey">Defines the type of the "Keys" which define the transitions.</typeparam>
    public class StateNode<TKey, T>
    {
        /// <summary>
        /// Gets or sets the value contianed by this node.
        /// </summary>
        public T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the graph that this node belongs to.
        /// </summary>
        public StateGraph<TKey, T> Graph
        {
            get;
            private set;
        }

        /// <summary>
        /// A list that contains the nodes that travel to this node.
        /// </summary>
        private Dictionary<TKey, StateNode<TKey, T>> toTransitions;

        /// <summary>
        /// Gets an array of nodes that travel to this node.
        /// </summary>
        public KeyValuePair<TKey, StateNode<TKey, T>>[] ToTransitions
        {
            get
            {
                return toTransitions.ToArray();
            }
        }

        /// <summary>
        /// Gets the transitions that defines which node to move to when a certian input(TKey) is recieved.
        /// The transitions that lead from this node.
        /// </summary>
        public KeyValuePair<TKey, StateNode<TKey, T>>[] FromTransitions
        {
            get
            {
                return fromTransitions.ToArray();
            }
        }

        /// <summary>
        /// The transitions that lead from this node.
        /// </summary>
        private Dictionary<TKey, StateNode<TKey, T>> fromTransitions;

        /// <summary>
        /// Adds the given key and value as a transition from this node to the given node.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddTransition(TKey key, StateNode<TKey, T> value)
        {
            //add a "from" transition so that the value knows
            //that we lead to it.
            if (!value.toTransitions.ContainsKey(key))
            {
                value.toTransitions.Add(key, this);
            }

            fromTransitions.Add(key, value);
        }

        /// <summary>
        /// Adds the given KeyValuePair as a transition.
        /// </summary>
        /// <param name="transition"></param>
        public void AddTransition(KeyValuePair<TKey, StateNode<TKey, T>> transition)
        {
            //add a "from" transition so that the value knows
            //that we lead to it.
            if (!transition.Value.toTransitions.ContainsKey(transition.Key))
            {
                transition.Value.toTransitions.Add(transition.Key, this);
            }
            fromTransitions.Add(transition.Key, transition.Value);
        }

        /// <summary>
        /// Removes a transition from Transitions based on the given key.
        /// This will remove all of the 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveTransition(TKey key)
        {
            //remove the link
            fromTransitions[key].toTransitions.Remove(key);
            return fromTransitions.Remove(key);
        }

        /// <summary>
        /// Gets the depth first traversal of the children of this node.
        /// </summary>
        /// <exception cref="System.StackOverflowException"/>
        /// <returns></returns>
        public IEnumerable<StateNode<TKey, T>> GetDepthFirstTraversal()
        {
            Stack<StateNode<TKey, T>> stack = new Stack<StateNode<TKey, T>>();
            Stack<StateNode<TKey, T>> traversal = new Stack<StateNode<TKey, T>>();
            traversal.Push(this);
            stack.Push(this);
            while (stack.Count != 0)
            {
                StateNode<TKey, T> node = stack.Pop();
                foreach (var transition in node.FromTransitions)
                {
                    stack.Push(transition.Value);
                    traversal.Push(transition.Value);
                }
            }
            return traversal;
        }

        /// <summary>
        /// Defines a queue that contains markers.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        private class MarkerQueue<Type>
        {
            /// <summary>
            /// Gets the internal queue that this object uses.
            /// </summary>
            public Queue<Marker<Type>> Queue
            {
                get;
                private set;
            }

            /// <summary>
            /// Creates a new MarkerQueue object.
            /// </summary>
            public MarkerQueue()
            {
                Queue = new Queue<Marker<Type>>();
            }
        }

        /// <summary>
        /// Defines a marker that determines if the value has already been visited.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        private struct Marker<Type>
        {
            /// <summary>
            /// Gets or sets the value contained by the marker.
            /// </summary>
            public Type Value
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets if this value is already marked.
            /// </summary>
            public bool Marked
            {
                get;
                set;
            }

            /// <summary>
            /// Creates a new marker with the given value and marked setting.
            /// </summary>
            /// <param name="value">The value that the marker should hold.</param>
            /// <param name="marked">Whether this object should be marked.</param>
            public Marker(Type value, bool marked = false)
                : this()
            {
                this.Value = value;
                this.Marked = marked;
            }
        }

        /// <summary>
        /// Gets the breadth first traversal of the graph.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StateNode<TKey, T>> GetBreadthFirstTraversal()
        {
            MarkerQueue<StateNode<TKey, T>> queue = new MarkerQueue<StateNode<TKey, T>>();
            Queue<StateNode<TKey, T>> traversal = new Queue<StateNode<TKey, T>>();
            traversal.Enqueue(this);
            queue.Queue.Enqueue(new Marker<StateNode<TKey, T>>(this));
            while (queue.Queue.Count != 0)
            {
                var node = queue.Queue.Dequeue();
                if (!node.Marked)
                {
                    foreach (var transition in node.Value.FromTransitions)
                    {
                        if (!traversal.Contains(transition.Value))
                        {
                            queue.Queue.Enqueue(new Marker<StateNode<TKey, T>>(transition.Value));
                            traversal.Enqueue(transition.Value);
                        }
                    }
                    node.Marked = true;
                }
            }
            return traversal;
        }

        /// <summary>
        /// Returns whether the given node is contained in a "from" transition.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsFromTransition(StateNode<TKey, T> node)
        {
            return GetBreadthFirstTraversal().Any(a => a == node);
        }

        /// <summary>
        /// Determines if this state contains a transition to another state based on the given key.
        /// </summary>
        /// <param name="key">The key of the transition.</param>
        /// <returns></returns>
        public bool ContainsFromTransition(TKey key)
        {
            return fromTransitions.ContainsKey(key);
        }

        /// <summary>
        /// Returns whether a transition is contained as a child of this node based on the given comparer.
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public bool ContainsFromTransition(Predicate<StateNode<TKey, T>> comparer)
        {
            return GetBreadthFirstTraversal().Any(a => comparer(a));
        }


        /// <summary>
        /// Gets a state that this state transitions to based on the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public StateNode<TKey, T> this[TKey key]
        {
            get
            {
                return fromTransitions[key];
            }
        }

        /// <summary>
        /// Gets the state node that the given key leads from.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public StateNode<TKey, T> FindToTransition(TKey key)
        {
            return toTransitions[key];
        }

        /// <summary>
        /// Gets a node from the "from" transitions that matches comparer. Returns null if the transition cannot be found.
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public StateNode<TKey, T> GetFromTransition(Predicate<StateNode<TKey, T>> comparer)
        {
            return GetBreadthFirstTraversal().FirstOrDefault(a => comparer(a));
        }

        /// <summary>
        /// Creates a new empty state.
        /// </summary>
        public StateNode()
        {
            fromTransitions = new Dictionary<TKey, StateNode<TKey, T>>();
            toTransitions = new Dictionary<TKey,StateNode<TKey,T>>();
            Value = default(T);
        }

        /// <summary>
        /// Creates a new state containing the given value.
        /// </summary>
        /// <param name="value"></param>
        public StateNode(T value)
        {
            fromTransitions = new Dictionary<TKey, StateNode<TKey, T>>();
            toTransitions = new Dictionary<TKey,StateNode<TKey,T>>();
            this.Value = value;
        }

        /// <summary>
        /// Creates a new state contianing the given value with a reference to the given graph.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="graph"></param>
        public StateNode(T value, StateGraph<TKey, T> graph)
        {
            fromTransitions = new Dictionary<TKey, StateNode<TKey, T>>();
            toTransitions = new Dictionary<TKey, StateNode<TKey, T>>();
            this.Value = value;
            this.Graph = graph;
        }

        /// <summary>
        /// Adds a transition between this state and a generated state based on the given key where the given value is the value of the generated state.
        /// </summary>
        /// <param name="key">The key that defines the transition.</param>
        /// <param name="val">The value of the new state to create.</param>
        public void AddTransition(TKey key, T val)
        {
            fromTransitions.Add(key, new StateNode<TKey, T>(val));
        }
    }
}
