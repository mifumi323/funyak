using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifuminSoft.funyak.Core
{
    public interface IBounds
    {
        double Left { get; }
        double Top { get; }
        double Right { get; }
        double Bottom { get; }
    }
}
