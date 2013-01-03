using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic;
using Utils;

namespace Editor
{
    public class FSMD
    {
        public int StartID { get; private set; }
        private List<StateD> states;
        private Dictionary<string, object> variables;

        public FSMD() 
        {
            states = new List<StateD>();
            this.variables = new Dictionary<string, object>();
        }

        internal int IndexOf(string stateID)
        {
            for (int i = 0; i < this.states.Count; i++)
            {
                if (this.states[i].Name == stateID)
                    return i;
            }

            return -1;
        }

        internal StateD this[string stateID]
        {
            get 
            {
                var result = this.states.Find((x) => x.Name == stateID);
                if (result == null)
                    throw new ArgumentException("No state called " + stateID + " exsists.");

                return result;
            }
        }

        internal bool StateExists(string stateID)
        {
            return IndexOf(stateID) != -1;
        }

        internal StateD GetState(string stateID)
        {
            if (!StateExists(stateID))
                throw new ArgumentException("No state named " + stateID + " exists.");

            return this.states.Find((x) => x.Name == stateID);
        }

        internal void SetStartState(string startState)
        {
            var index = this.IndexOf(startState);
            if (index == -1)
                throw new ArgumentException("No state called " + startState + " exists. So it can not be used to set the start state.");

            this.StartID = index;
        }

        internal void NewState(string name)
        {
            if (this.StateExists(name))
                throw new ArgumentException("A state called " + name + " already exists.");

            var state = new StateD(this, name);
            this.states.Add(state);
        }

        internal void RemoveState(string stateID)
        {
            int index = this.IndexOf(stateID);
            if (index == -1)
                throw new ArgumentException("No state called " + stateID + " exists.");

            this.states.RemoveAt(index);
            this.FixStateTransitions(stateID);
        }

        private void FixStateTransitions(string stateID)
        {
            foreach (var state in this.states)
            {
                state.RemoveTransitionsTo(stateID);
            }
        }

        internal void AddVariable(string variableName, object value)
        {
            if (this.variables.ContainsKey(variableName))
                throw new ArgumentException("A variable called " + variableName + " already exists.");
            if (value == null)
                throw new NullReferenceException("value");

            this.variables.Add(variableName, value);

        }

        internal IFSM ToFsm()
        {
            State[] states = new State[this.states.Count];
            for (int i = 0; i < states.Length; i++)
            {
                states[i] = this.states[i].ToState();
            }

            var fsm = new FSM(new VariableCollection(variables), states, this.StartID);
            return fsm;
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("State Machine");
            foreach (var state in this.states)
            {
                builder.AppendLine("  " + state.ToString());
            }
            return builder.ToString();
        }
    }

    public class StateD
    {
        private FSMD owner;
        private string name;
        private Dictionary<string, string> transitions;
        private List<IStateAction> actions;

        public string Name 
        {
            get { return name; }
            set
            {
                if (this.owner.StateExists(value))
                    throw new ArgumentException("Cannot rename to " + value + " since a state with that name already exists.");
                else if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Name cannot be empty or whitespace.");

                this.name = value;
            }
        }

        public StateD(FSMD owner, string name)
        {
            this.name = name;
            this.owner = owner;
            this.transitions = new Dictionary<string, string>();
            this.actions = new List<IStateAction>();
        }

        public void AddTranstion(string _event, string stateID)
        {
            if (this.transitions.ContainsKey(_event))
                throw new ArgumentException("A transition already exists for the event " + _event + ".");
            else if (!this.owner.StateExists(stateID))
                throw new ArgumentException("No such state " + stateID);

            this.transitions.Add(_event, stateID);
        }

        public void RemoveTransition(string _event)
        {
            this.transitions.Remove(_event);
        }

        public void RemoveTransitionsTo(string stateID)
        {
            List<string> toRemove = new List<string>();
            foreach (var trans in this.transitions)
            {
                if (trans.Value == stateID)
                    toRemove.Add(trans.Key);
            }
            toRemove.ForEach((x) => transitions.Remove(x));
        }

        public void AddAction(IStateAction action)
        {
            this.actions.Add(action);
        }

        public void RemoveAction(IStateAction action)
        {
            this.actions.Remove(action);
        }
                                
        internal State ToState()
        {
            var trans = GetTransistions();
            return new State(this.Name, actions, trans);
        }

        private IDictionary<string,int> GetTransistions()
        {
            var dict = new Dictionary<string, int>();
            foreach (var tran in this.transitions)
            {
                dict.Add(tran.Key, owner.IndexOf(tran.Value));
            }
            return dict;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(name);
            builder.AppendLine("    Transitions:");
            foreach (var transition in this.transitions)
            {
                builder.AppendLine("      " + transition.Key + " : " + transition.Value);
            }
            builder.AppendLine("    Actions:");
            foreach (var action in this.actions)
            {
                builder.AppendLine("      " + action);
            }

            return builder.ToString();
        }
    }
}
