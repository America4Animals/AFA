using AFA.ServiceModel;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface IStateProvincesService
    {
        StateProvincesResponse Get(StateProvinces request);
        IRequestContext RequestContext { get; set; }
    }
}