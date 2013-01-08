using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceModel;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    [Route("/reset-db", "GET,POST")]
    public class ResetDb { }

    public class ResetDbResponse
    {
    }

    public class ResetDbService : ServiceStack.ServiceInterface.Service
    {
        public object Any(ResetDb request)
        {
            Db.CreateTableIfNotExists<StateProvince>();
            Db.CreateTableIfNotExists<OrganizationCategory>();
            Db.CreateTableIfNotExists<Organization>();
            Db.CreateTableIfNotExists<OrganizationOrganizationCategory>();
            Db.CreateTableIfNotExists<User>();

            if (Db.Select<StateProvince>().Count == 0)
            {
                var newYork = new StateProvince
                {
                    Abbreviation = "NY",
                    Name = "New York"
                };

                var california = new StateProvince
                {
                    Abbreviation = "CA",
                    Name = "California"
                };

                Db.Insert(newYork);
                Db.Insert(california);
            }

            return new ResetDbResponse();
        }
    }
}