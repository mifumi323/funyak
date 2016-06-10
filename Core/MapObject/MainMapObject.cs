﻿using System;
using MifuminSoft.funyak.Core.Input;

namespace MifuminSoft.funyak.Core.MapObject
{
    public class MainMapObject : IDynamicMapObject
    {
        protected abstract class IState
        {
            public abstract double GetLeft(double x);
            public abstract double GetTop(double y);
            public abstract double GetRight(double x);
            public abstract double GetBottom(double y);

            public virtual void UpdateSelf(MainMapObject main)
            {
            }
        }

        protected class FloatingState : IState
        {
            public override double GetLeft(double x)
            {
                return x - 14;
            }

            public override double GetTop(double y)
            {
                return y - 14;
            }

            public override double GetRight(double x)
            {
                return x + 14;
            }

            public override double GetBottom(double y)
            {
                return y + 14;
            }
        }

        protected IState State { get; private set; }

        public IInput Input { get; set; }

        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }

        public double Left
        {
            get { return State.GetLeft(X); }
        }

        public double Top
        {
            get { return State.GetTop(Y); }
        }

        public double Right
        {
            get { return State.GetRight(X); }
        }

        public double Bottom
        {
            get { return State.GetBottom(Y); }
        }

        public int Appearance { get; set; }

        public MainMapObject(double x, double y)
        {
            State = new FloatingState();
            Input = new NullInput();
            X = x;
            Y = y;
        }

        public void UpdateSelf()
        {
            State.UpdateSelf(this);
        }

        public Action CheckCollision()
        {
            return null;
        }
    }
}
