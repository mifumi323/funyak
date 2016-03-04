using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifuminSoft.funyak.Core.MapObject
{
    class MainMapObject : IMapObject
    {
        protected abstract class IState
        {
            public abstract double GetLeft(double x);
            public abstract double GetTop(double y);
            public abstract double GetRight(double x);
            public abstract double GetBottom(double y);
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

        public double X
        {
            get;
            protected set;
        }

        public double Y
        {
            get;
            protected set;
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

        public MainMapObject()
        {
            State = new FloatingState();
        }
    }
}
