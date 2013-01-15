using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Monocle.Utils;

namespace Monocle.Core
{
    public abstract class Behaviour : Component, IUpdatable, IFixUpdatable, IStartable, IResetable, ICoroutine
    {
        private List<IEnumerator> coroutines;

        public Behaviour() : base()
        {
            this.coroutines = new List<IEnumerator>();
        }

        public void StartCoroutine(IEnumerable corutine)
        {
            if (corutine == null)
                throw new ArgumentNullException("coroutine");
            coroutines.Add(corutine.GetEnumerator());
        }

        public void StepCoroutines()
        {
            for (int i = coroutines.Count - 1; i >= 0; i--)
            {
                IEnumerator itr = coroutines[i];
                if (!itr.MoveNext())
                    coroutines.RemoveAt(i);
            }
        }

        public static IEnumerable WaitSeconds(double seconds)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.Elapsed.TotalSeconds < seconds)
                yield return 0;
        }

        public virtual void Start() { }
        public virtual void Awake() { }
        public virtual void Reset() { }
        public virtual void Update(Time time) { }
        public virtual void FixedUpdate(Time time) { }
        public virtual void OnDestroyed() { }
    }
}
