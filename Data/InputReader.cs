using MifuminSoft.funyak.Input;
using Newtonsoft.Json;

namespace MifuminSoft.funyak.Data
{
    class InputReader
    {
        public static IInput FromString(string data, MapReaderOption option)
        {
            return FromDynamic(JsonConvert.DeserializeObject(data), option);
        }

        public static IInput FromDynamic(dynamic data, MapReaderOption option)
        {
            // TODO: ReplayInputも作ろう
            return
                data.type == "arrange" ? GenerateArrangeInput(data, option) :
                null;
        }

        private static IInput GenerateArrangeInput(dynamic data, MapReaderOption option)
        {
            var input = new ArrangeInput(option.Input)
            {
                HorizontalReverse = data.hr ?? false,
                VerticalReverse = data.vr ?? false,
            };
            return input;
        }
    }
}
