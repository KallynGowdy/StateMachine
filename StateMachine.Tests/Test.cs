using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using KallynGowdy;
using KallynGowdy.StateMachine;

namespace Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        //[ExpectedException(typeof(ArgumentException))]
        public void Test1()
        {
            var builder = new DeterministicFiniteAutoma<char>.Builder(0);


            DeterministicFiniteAutoma<char> dfa = builder.To('a', 1).BeginGroup("b's").To('b', 2).To('b', 3).EndGroup("b's").To(
                new Dictionary<char, int>()
                {
                    {'b', 2},
                    {'c', 5}
                }).Build();

                builder.To('a', 0).To('b', 1).To('b', 2);
                builder.To
                (
                    new Dictionary<char, int>()
                    {
                        {'b', 1},
                        {'c', 4}
                    }
                );


            dfa.OnGroupEntered += a =>
            {
                Console.WriteLine("{0} Entered", a);
            };

            dfa.OnGroupExited += (a, b) =>
            {

                Console.WriteLine("{0} Exited", a);
            };

            dfa.Run("abbbbc");
        }
    }
}