using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceModel;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    /// <summary>
    /// GET /state-provinces
    /// Returns a list of state provinces
    /// </summary>
    public class StateProvincesService : ServiceStack.ServiceInterface.Service, IStateProvincesService
    {
        public StateProvincesResponse Get(StateProvinces request)
        {
            return new StateProvincesResponse
            {
                StateProvinces = Db.Select<StateProvince>()
            };
        }
    }
}
