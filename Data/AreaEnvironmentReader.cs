using System;
using MifuminSoft.funyak.MapEnvironment;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public class AreaEnvironmentReader
    {
        public static AreaEnvironment FromString(string data, MapReaderOption option)
        {
            return FromDynamic(JsonConvert.DeserializeObject(data), option);
        }

        public static AreaEnvironment FromDynamic(dynamic data, MapReaderOption option)
        {
            return new AreaEnvironment()
            {
                Left = data.l ?? double.NegativeInfinity,
                Top = data.t ?? double.NegativeInfinity,
                Right = data.r ?? double.PositiveInfinity,
                Bottom = data.b ?? double.PositiveInfinity,

                Gravity = data.g ?? 1.0,
                Wind = data.w ?? 0.0,
            };
        }
    }
}