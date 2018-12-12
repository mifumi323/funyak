namespace MifuminSoft.funyak.Input
{
    /// <summary>入力なし</summary>
    public class NullInput : IInput
    {
        public double X => 0;

        public double Y => 0;

        public bool IsPressed(Keys key) => false;

        public bool IsPushed(Keys key) => false;

        public bool IsReleased(Keys key) => false;

        public void Update() { }
    }
}
