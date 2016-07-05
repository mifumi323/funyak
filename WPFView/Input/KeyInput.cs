using System.Windows.Input;
using MifuminSoft.funyak.Input;

namespace MifuminSoft.funyak.View.Input
{
    /// <summary>
    /// キー入力のとりあえずの実装
    /// </summary>
    public class KeyInput : InputBase
    {
        protected override void UpdateImpl()
        {
            double x = 0, y = 0;
            if (Keyboard.IsKeyDown(Key.Left)) x -= 1;
            if (Keyboard.IsKeyDown(Key.Up)) y -= 1;
            if (Keyboard.IsKeyDown(Key.Right)) x += 1;
            if (Keyboard.IsKeyDown(Key.Down)) y += 1;
            SetDirection(x, y);
            SetKey(Keys.Jump, Keyboard.IsKeyDown(Key.Z));
            SetKey(Keys.Attack, Keyboard.IsKeyDown(Key.X));
            SetKey(Keys.Smile, Keyboard.IsKeyDown(Key.C));
        }
    }
}
