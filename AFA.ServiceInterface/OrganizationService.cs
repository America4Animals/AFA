using System.Linq;
using AFA.ServiceModel;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    public class OrganizationService : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// GET /organizations/{Id}
        /// </summary>
        public object Get(Organization organization)
        {
            return new OrganizationResponse
                       {
                           Organization = Db.Id<Organization>(organization.Id)
                       };
        }

        public object Post(Organization organization)
        {
            Db.Insert(organization);
            //return new HttpResult(Db.GetLastInsertId(), HttpStatusCode.Created);
            //return new HttpResult(new {id = Db.GetLastInsertId()}, HttpStatusCode.Created);
            return new OrganizationResponse {Organization = new Organization()};
        }

        public object Put(Organization organization)
        {
            Db.Update(organization);
            //return new HttpResult { StatusCode = HttpStatusCode.NoContent };
            return new OrganizationResponse { Organization = new Organization() };
        }

        public object Delete(Organization organization)
        {
            Db.DeleteById<Organization>(organization.Id);
            //return new HttpResult { StatusCode = HttpStatusCode.NoContent };
            return new OrganizationResponse { Organization = new Organization() };
        }
    }

    /// <summary>
    /// GET /organizations
    /// GET /organizations/category/{CategoryId}
    /// Returns a list of organizations
    /// </summary>
    public class OrganizationsService : ServiceStack.ServiceInterface.Service
    {
        public object Get(Organizations request)
        {
            return new OrganizationsResponse
            {
                //Organizations = request.CategoryId.HasValue ? 
                //    Db.Select<Organization>().Where(o => o.Categories.Any(c => c.Id == request.CategoryId.Value)).ToList() : Db.Select<Organization>()

                Organizations = Db.Select<Organization>()
            };
        }
    }
}