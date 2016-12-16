using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public class PermanantStates<T>
    {
        private Dictionary<string, T> states;
        private string currentName;
        public string CurrentName
        {
            get
            {
                return currentName;
            }
            set
            {
                if (states.ContainsKey(value))
                {
                    currentName = value;
                    CurrentState = states[CurrentName];
                }
            }
        }
        public T CurrentState { get; private set; }

        public PermanantStates()
        {
            states = new Dictionary<string, T>();
            currentName = null;
            CurrentState = default(T);
        }

        public void AddState(string name, T state)
        {
            if (!states.ContainsKey(name))
            {
                states.Add(name, state);
            }

            if (CurrentName == null)
            {
                CurrentName = name;
            }
        }

        public T GetState(string name)
        {
            if (states.ContainsKey(name))
            {
                return states[name];
            }
            else
            {
                throw new Exception("That state doesn't exist");
            }
        }

        public void RemoveState(string name)
        {
            if (states.ContainsKey(name))
            {
                if (CurrentName == name)
                {
                    CurrentName = null;
                    CurrentState = default(T);
                }
                states.Remove(name);
            }
        }
    }
}
