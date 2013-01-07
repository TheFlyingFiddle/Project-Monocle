using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework;
using Utils;

namespace Logic
{
    class FSMComponent : Behaviour, IFSM
    {
        public class State : IState
        {
            private FSMComponent fsm;
            private List<IStateAction> actions;
            private Dictionary<string, string> transitions;
            private string name;
            
            internal State(State state, FSMComponent fsm)
            {
                this.name = state.Name;
                this.fsm = fsm;
                this.actions = state.actions.Select((x) => (IStateAction)x.Clone()).ToList();
                this.transitions = new Dictionary<string, string>(state.transitions);
            }

            internal State(State state, string newName)
            {
                this.name = newName;
                this.fsm = state.fsm;
                this.actions = state.actions.Select((x) => (IStateAction)x.Clone()).ToList();
                this.transitions = new Dictionary<string, string>(state.transitions);
            }

            internal State(string name, FSMComponent fsm)
            {
                this.name = name;
                this.fsm = fsm;
                this.actions = new List<IStateAction>();
                this.transitions = new Dictionary<string, string>();
            }

            public string Name
            {
                get { return this.name; }
                set
                {
                    this.Rename(value);
                }
            }

            private void Rename(string name)
            {
                this.fsm.RenameState(this.name, name);
                this.name = name;
            }
            
            internal string GetTransition(string _event)
            {
                if (this.transitions.ContainsKey(_event))
                    return this.transitions[_event];

                return string.Empty;
            }

            public void Enter()
            {
                this.actions.ForEach((x) => x.Enter());
            }

            public void Exit()
            {
                this.actions.ForEach((x) => x.Exit());
            }

            public IEnumerable<IStateAction> Actions
            {
                get { return actions; }
            }

            public T AddAction<T>() where T : IStateAction,  new()
            {
                var action = new T();
                action.FSM = this.fsm;
                this.actions.Add(action);
                return action;
            }

            public bool RemoveAction(IStateAction toRemove)
            {
                return this.actions.Remove(toRemove);
            }

            public void AddTransition(string _event, string to)
            {
                if (!this.fsm.states.ContainsKey(to))
                    throw new ArgumentException(string.Format("Cannot add a transition to {0} since it does not exsist.", to));

                this.transitions.Add(_event, to);
            }

            public bool RemoveTransition(string _event)
            {
                return this.transitions.Remove(_event);
            }

            public void SendMessage(string messageID, object param = null, MessageOptions opt = MessageOptions.DontRequireReceiver)
            {
                this.actions.ForEach((x) => x.SendMessage(messageID, param, opt));
            }

            public State Copy()
            {
                string name = fsm.GetCopyName(this.name);
                var state = Copy(name);
                this.fsm.states.Add(state.name, state);
                return state;
            }

            internal State Copy(string newName)
            {
                return new State(this, newName);
            }

            internal State Clone(FSMComponent fsm)
            {
                return new State(this, fsm);
            }

            internal void RemoveTransitionsToState(string name)
            {
                var toRemove = new List<string>();
                foreach (var item in this.transitions)
                {
                    if (item.Value == name)
                        toRemove.Add(item.Key);
                }
                toRemove.ForEach((x) => transitions.Remove(x));
            }

            internal void ChangeTransitions(string oldName, string newName)
            {
                var toChange = new List<string>();
                foreach (var item in this.transitions)
                {
                    if (item.Value == oldName)
                        toChange.Add(item.Key);
                }

                toChange.ForEach(x => transitions[x] = newName);
            }
        }

        private readonly IVariableCollection variables;
        private readonly Dictionary<string, State> states;
        private string startState;
        private State currentState;

        public FSMComponent()
        {
            this.variables = new VariableCollection();
            this.states = new Dictionary<string, State>();
        }

        public FSMComponent(FSMComponent fSMComponent)
        {
            this.startState = fSMComponent.startState;
            this.variables = (IVariableCollection)fSMComponent.variables.Clone();
            this.states = CloneStates(fSMComponent.states);

        }

        private Dictionary<string, State> CloneStates(Dictionary<string, State> dictionary)
        {
            var dict = new Dictionary<string, State>();
            dictionary.ForEach((x) => dict.Add(x.Key, (State)x.Value.Clone(this)));
            return dict;
        }

        protected override Component Clone()
        {
            return new FSMComponent(this);
        }
        
        public void Start()
        {
            if (this.currentState != null)
                this.currentState.Exit();

            if (this.startState != null)
            {
                this.currentState = states[startState];
                this.currentState.Enter();
            }
        }

        public void SendEvent(string _event)
        {
            var result = this.currentState.GetTransition(_event);
            if (result != string.Empty)
                this.Transition(result);          
        }

        private void Transition(string state)
        {
            this.currentState.Exit();
            this.currentState = this.states[state];
            this.currentState.Enter();
        }

        public State AddState(string name)
        {
            if (this.states.ContainsKey(name))
                throw new ArgumentException(string.Format("State {0} already exists.", name));

            if (string.IsNullOrEmpty(startState))
                this.startState = name;

            var state = new State(name, this);
            this.states.Add(name, state);
            return state;
        }

        public bool RemoveState(string name)
        {
            var result = this.states.Remove(name);
            if (result)
                this.states.Values.ForEach((x) => x.RemoveTransitionsToState(name));

            return result;
        }

        public State GetState(string name)
        {
            if (!this.states.ContainsKey(name))
                throw new ArgumentException(string.Format("State {0} does not exist.", name));

            return this.states[name];
        }

        internal string GetCopyName(string nameToCopy)
        {
            string copyName = nameToCopy + "-Copy";
            for (int i = 2; this.states.ContainsKey(copyName) ; i++)
            {
                copyName = nameToCopy + "-Copy " + i;
            }

            return copyName;
        }

        internal void RenameState(string oldName, string newName)
        {
            if (this.states.ContainsKey(newName))
                throw new ArgumentException(string.Format("Cannot rename {0} to {1} since the state {1} already exists.", oldName, newName));

            var state = this.states[oldName];
            this.states.Remove(oldName);
            this.states.Add(newName, state);

            foreach (var state1 in this.states.Values)
            {
                state1.ChangeTransitions(oldName, newName);
            }
        }

        public Variable<T> GetVariable<T>(string variableID)
        {
            return this.variables.GetVariable<T>(variableID);
        }

        public Variable<T> AddVariable<T>(string variableID, T value)
        {
            return this.variables.AddVariable(variableID, value);
        }

        public bool RemoveVariable(string variableID)
        {
            return this.variables.RemoveVariable(variableID);
        }

        public bool HasVariable(string name)
        {
            return this.variables.HasVariable(name);
        }

        public void SetStartState(string stateID)
        {
            if (!this.states.ContainsKey(stateID))
                throw new ArgumentException(string.Format("Cannot set the startstate to {0} siince it dosn't exsist.", stateID));

            this.startState = stateID;
        }
    }
}