using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic;

namespace Editor
{
    /// <summary>
    /// Simple class for using custom code on state transitions.
    /// </summary>
    class DelegateAction : StateAction
    {
        private readonly Action<IFSM> enterAction;
        private readonly Action<IFSM> exitAction;

        /// <summary>
        /// Creates a DelegateAction
        /// </summary>
        /// <param name="enterAction">The action that will be called when the action enters.</param>
        /// <param name="exitAction">The action that will be called when the action exits. Not required.</param>
        public DelegateAction(Action<IFSM> enterAction, Action<IFSM> exitAction = null)
        {
            if (enterAction == null)
                throw new ArgumentNullException("action");

            this.enterAction = enterAction;
            this.exitAction = exitAction;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="action"></param>
        private DelegateAction(DelegateAction action)
        {
            this.enterAction = action.enterAction;
            this.exitAction = action.exitAction;
        }

        /// <summary>
        /// Initialisation logic. Called when the action enters.
        /// </summary>
        public override void Enter()
        {
            this.enterAction(this.FSM);
        }

        /// <summary>
        /// Teardown logic.
        /// </summary>
        public override void Exit()
        {
            if (this.exitAction != null)
                this.exitAction(this.FSM);
        }

        /// <summary>
        /// Clones the instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new DelegateAction(this);
        }
    }
}
