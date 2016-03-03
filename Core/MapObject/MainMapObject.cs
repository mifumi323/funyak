using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifuminSoft.funyak.Core.MapObject
{
    class MainMapObject : IMapObject
    {
        public double Bottom
        {
            get;
            protected set;
        }

        public double Left
        {
            get;
            protected set;
        }

        public double Right
        {
            get;
            protected set;
        }

        public double Top
        {
            get;
            protected set;
        }

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
    }
}
