namespace MifuminSoft.funyak.View.AreaEnvironment
{
    public class AreaEnvironmentViewFactory
    {
        public IAreaEnvironmentView Create(MapEnvironment.AreaEnvironment areaEnvironment)
            => areaEnvironment is MapEnvironment.AreaEnvironment ? new AreaEnvironmentView(areaEnvironment) : null;
    }
}
