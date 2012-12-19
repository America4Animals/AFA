using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.Service.Models;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;

namespace AFA.Service.Services
{
    [Route("/reset-db", "GET,POST")]
    public class ResetMovies { }

    public class ResetDbResponse
    {
    }

    public class ResetDbService : ServiceStack.ServiceInterface.Service
    {
        public object Any(ResetMovies request)
        {
            Db.DropAndCreateTable<OrganizationCategory>();
            return new ResetDbResponse();
        }
    }
}