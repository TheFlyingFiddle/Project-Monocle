﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Utils;

namespace Monocle.Core
{
    public interface IUpdatable
    {
        void Update(Time time);
    }

    public interface IFixUpdatable
    {
        void FixedUpdate(Time time);
    }

    public interface IStartable
    {
        void Start();
    }

    public interface IAwakable
    {
        void Awake();
    }

    public interface IResetable
    {
        void Reset();
    }

    public interface ICoroutine
    {
        void StepCoroutines();
    }
}