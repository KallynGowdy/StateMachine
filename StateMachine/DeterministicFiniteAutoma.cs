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
    /// Defines a deterministic state machine that, given a state graph can process input.
    /// </summary>
    /// <typeparam name="T">The type of input to process.</typeparam>
    public class DeterministicFiniteAutoma<T>
    {
        /// <summary>
        /// Defines a builder class that helps with creating a DeterministicFiniteAutoma object.
        /// </summary>
        public class Builder : IStateGraphBuilder<Builder, T, int>
        {
            private Dictionary<string, int> groups;

            private List<IGroup> completedGroups;

            public Builder(int startState = 0)
            {
                groups = new Dictionary<string, int>();
                completedGroups = new List<IGroup>();
                CurrentNode = new StateNode<T, int>(startState);
                Graph = new StateGraph<T, int>(CurrentNode);
            }

            /// <summary>
            /// Marks the beginning of a between group.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Builder BeginGroup(string name)
            {
                groups.Add(name, CurrentNode.Value);
                return this;
            }

            /// <summary>
            /// Marks the end of a between group.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Builder EndGroup(string name)
            {
                completedGroups.Add(new BetweenGroup(name, new FromTo<int>(groups[name], CurrentNode.Value)));
                groups.Remove(name);
                return this;
            }

            private void endAllGroups()
            {
                foreach (KeyValuePair<string, int> group in groups)
                {
                    EndGroup(group.Key);
                }
            }

            /// <summary>
            /// Builds the data contained in this builder object into a DeterministicFiniteAutoma state machine.
            /// </summary>
            /// <returns></returns>
            public DeterministicFiniteAutoma<T> Build()
            {
                DeterministicFiniteAutoma<T> automa = new DeterministicFiniteAutoma<T>(Graph);
                endAllGroups();
                foreach (IGroup group in completedGroups)
                {
                    automa.Groups.Add(group);
                }
                return automa;
            }

            public Builder Back(T key)
            {
                CurrentNode = CurrentNode.FindToTransition(key);
                return this;
            }

            public StateNode<T, int> CurrentNode
            {
                get;
                private set;
            }

            public StateGraph<T, int> Graph
            {
                get;
                private set;
            }

            public Builder To(IEnumerable<KeyValuePair<T, StateNode<T, int>>> nodes)
            {

                foreach (KeyValuePair<T, StateNode<T, int>> keyVal in nodes)
                {
                    CurrentNode.AddTransition(keyVal.Key, keyVal.Value);

                }
                return this;
            }

            public Builder To(IEnumerable<KeyValuePair<T, int>> nodes)
            {
                foreach (KeyValuePair<T, int> keyVal in nodes)
                {
                    StateNode<T, int> node = Graph.FindExistingNode(a => a.Value == keyVal.Value);
                    if (node != null)
                    {
                        CurrentNode.AddTransition(keyVal.Key, node);
                    }
                    else
                    {
                        Console.WriteLine("{0} Null", keyVal);
                        CurrentNode.AddTransition(keyVal.Key, keyVal.Value);
                    }
                }
                return this;
            }

            /// <summary>
            /// Adds a transition from the current state to the given state based on the given key and then moves to the created state.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="node"></param>
            /// <returns></returns>
            public Builder To(T key, StateNode<T, int> node)
            {
                CurrentNode.AddTransition(key, node);
                CurrentNode = node;
                return this;
            }

            /// <summary>
            /// Adds a transition from the current state to the given state based on the key and value and then moves to the created state.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public Builder To(T key, int value)
            {
                StateNode<T, int> newNode = new StateNode<T, int>(value, Graph);

                CurrentNode.AddTransition(key, newNode);

                CurrentNode = newNode;

                return this;
            }

            /// <summary>
            /// Moves to the next state based on the given key or creates a new node whose state is one more than the current state.
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Builder To(T key)
            {
                if (CurrentNode.ContainsFromTransition(key))
                {
                    CurrentNode = CurrentNode[key];

                    return this;
                }
                StateNode<T, int> newNode = new StateNode<T, int>(CurrentNode.Value + 1, Graph);

                CurrentNode.AddTransition(key, newNode);

                CurrentNode = newNode;

                return this;
            }


            StateGraph<T, int> IStateGraphBuilder<Builder, T, int>.Build()
            {
                endAllGroups();
                return Graph;
            }
        }

        StateGraph<T, int> stateMachine;

        /// <summary>
        /// Gets a list of groups that determine 
        /// </summary>
        public IList<IGroup> Groups
        {
            get;
            private set;
        }

        public event Action<IGroup> OnGroupEntered;

        public event Action<IGroup, IEnumerable<T>> OnGroupExited;

        public DeterministicFiniteAutoma(StateGraph<T, int> graph)
        {
            this.stateMachine = graph;
            Groups = new List<IGroup>();
        }

        /// <summary>
        /// Runs the Deterministic Finite State Automa on the given input.
        /// </summary>
        /// <param name="input">The input to match to the state machine.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public void Run(IEnumerable<T> input)
        {
            StateNode<T, int> currentNode = stateMachine.Root;
            int currentState = currentNode.Value;
            Console.WriteLine("Start State: {0}", currentState);
            Dictionary<IGroup, List<T>> currentGroups = new Dictionary<IGroup, List<T>>();
            foreach (T val in input)
            {
                Console.WriteLine(val);

                if (currentNode.ContainsFromTransition(val))
                {
                    currentNode = currentNode[val];
                    currentState = currentNode.Value;
                    Console.WriteLine(currentState);
                    //Add value to current groups
                    foreach (List<T> l in currentGroups.Values)
                    {
                        l.Add(val);
                    }

                    //Check entering groups
                    foreach (IGroup group in Groups.Where(a => a.In(currentState)).Except(currentGroups.Select(a => a.Key)).ToArray())
                    {
                        invokeOnGroupEntered(group);
                        currentGroups.Add(group, new List<T>());
                    }

                    //Check exiting groups
                    foreach (KeyValuePair<IGroup, List<T>> group in currentGroups.Where(a => !a.Key.In(currentState)).ToArray())
                    {
                        invokeOnGroupExited(group);
                        currentGroups.Remove(group.Key);
                    }
                }
                else
                {
                    throw new ArgumentException("Input Error. The given input does not match the current state graph", "input");
                }
            }
        }


        private void invokeOnGroupExited(KeyValuePair<IGroup, List<T>> group)
        {
            Action<IGroup, IEnumerable<T>> temp = OnGroupExited;
            if (temp != null)
            {
                temp(group.Key, group.Value);
            }
        }

        private void invokeOnGroupEntered(IGroup group)
        {
            Action<IGroup> temp = OnGroupEntered;
            if (temp != null)
            {
                temp(group);
            }
        }
    }
}
