namespace MifuminSoft.funyak.View.AreaEnvironment
{
    public class AreaEnvironmentViewFactory
    {
        public IAreaEnvironmentView Create(MapEnvironment.AreaEnvironment areaEnvironment)
        {
            if (areaEnvironment is MapEnvironment.AreaEnvironment)
            {
                return new AreaEnvironmentView(areaEnvironment);
            }
            return null;
        }
    }
}
