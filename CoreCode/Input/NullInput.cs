namespace MifuminSoft.funyak.Input
{
    /// <summary>入力なし</summary>
    public class NullInput : IInput
    {
        public double X
        {
            get
            {
                return 0;
            }
        }

        public double Y
        {
            get
            {
                return 0;
            }
        }

        public bool IsPressed(Keys key)
        {
            return false;
        }

        public bool IsPushed(Keys key)
        {
            return false;
        }

        public bool IsReleased(Keys key)
        {
            return false;
        }

        public void Update()
        {
        }
    }
}
