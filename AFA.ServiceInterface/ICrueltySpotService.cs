using AFA.ServiceModel.DTOs;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface ICrueltySpotService
    {
        /// <summary>
        /// GET /crueltyspots/{Id}
        /// </summary>
        CrueltySpotResponse Get(CrueltySpotDto request);

        object Post(CrueltySpotDto request);
        object Put(CrueltySpotDto crueltySpotDto);
        object Delete(CrueltySpotDto crueltySpotDto);
        IRequestContext RequestContext { get; set; }
    }

    public interface ICrueltySpotsService
    {
        CrueltySpotsResponse Get(CrueltySpotsDto request);
        IRequestContext RequestContext { get; set; }
    }
}