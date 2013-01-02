using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using Logic;
namespace Editor
{
    public class FSMEditor
    {
        private FSMD description;

        public FSMEditor()
        {
            description = new FSMD();
        }

        public FSM BuildFSM()
        {
            string line;
            while ((line = Console.ReadLine()) != "done")
            {
                HandleCommand(line.Split(' '));
            }

            return (FSM)description.ToFsm();
        }


        public void HandleCommand(string[] command)
        {
            try
            {
                InvokeDynamic(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InvokeDynamic(string[] methodDescription)
        {
            var methodInfo = this.GetType().GetMethod(methodDescription[0].ToLower(), BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
                throw new ArgumentException("No command called " + methodDescription[0]);

            if (methodInfo.GetParameters().Length != methodDescription.Length - 1)
                throw new ArgumentException("Invalid parameter count for " + methodDescription[0]);

            var t = methodDescription.ToList();
            t.RemoveAt(0);

            try
            {
                methodInfo.Invoke(this, t.ToArray());
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        private void ns(string p)
        {
            this.description.NewState(p);
            Console.WriteLine("A new state " + p + " was jsut added.");
        }

        private void re(string p, string p_2)
        {
            this.description.GetState(p).Name = p_2;
            Console.WriteLine(p + " was renamed to " + p_2 + ".");
        }

        private void ca(string state, string _command, string _event)
        {
            this.description.GetState(state).AddAction(new ConsoleAction(_command, _event));
            Console.WriteLine("Console Action added.");
        }

        private void wa(string state, string message)
        {
            this.description.GetState(state).AddAction(new DelegateAction((x) => Console.WriteLine(message)));
            Console.WriteLine("Added write action");
        }

        private void wsa(string state, string message)
        {
            this.description.GetState(state).AddAction(new DelegateAction((x) => WriteSlow(x, message)));
            Console.WriteLine("Added write action");
        }


        private void WriteSlow(IFSM fsm, string toWrite)
        {
            Console.Write(toWrite);
            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Console.WriteLine();
            fsm.SendEvent("finished");

        }


        private void del(string state)
        {
            this.description.RemoveState(state);
            Console.WriteLine(state + " was jsut deleted.");
        }

        private void tr(string _event, string start_state, string end_state)
        {
            this.description.GetState(start_state).AddTranstion(_event, end_state);
            Console.WriteLine("Added a transition.");
        }

        private void clr()
        {
            Console.Clear();
        }

        private void print()
        {
            Console.WriteLine(this.description);
        }
    }
}
