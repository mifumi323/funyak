using MifuminSoft.funyak.MapEnvironment;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    public class AreaEnvironmentReader
    {
        public static AreaEnvironment FromString(string data, MapReaderOption option) => FromDynamic(JsonConvert.DeserializeObject(data), option);

        public static AreaEnvironment FromDynamic(dynamic data, MapReaderOption option) => new AreaEnvironment()
        {
            Name = data.n ?? null,

            Left = data.l ?? double.NegativeInfinity,
            Top = data.t ?? double.NegativeInfinity,
            Right = data.r ?? double.PositiveInfinity,
            Bottom = data.b ?? double.PositiveInfinity,

            Gravity = data.g ?? double.NaN,
            Wind = data.w ?? double.NaN,

            BackgroundColor = data.c,
        };
    }
}