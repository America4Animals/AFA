using System.Linq;
using AFA.ServiceInterface.Mappers;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    public class OrganizationService : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// GET /organizations/{Id}
        /// </summary>
        public object Get(OrganizationDto organizationDto)
        {
            var query = string.Format("select o.*, oc.Name, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation " +
                                      "from Organization o " +
                                      "left join OrganizationOrganizationCategory ooc " +
                                      "on o.Id = ooc.OrganizationId " +
                                      "left join OrganizationCategory oc " +
                                      "on ooc.OrganizationCategoryId = oc.Id " +
                                      "left join StateProvince sp " +
                                      "on o.StateProvinceId = sp.Id " +
                                      "where o.Id = {0}", organizationDto.Id);

            return new OrganizationResponse
                       {
                           Organization = Db.Select<OrganizationDto>(query).FirstOrDefault()
                       };
        }

        public object Post(OrganizationDto organizationDto)
        {
            var organization = organizationDto.ToEntity();
            Db.Insert(organization);
            return new OrganizationResponse { Organization = new OrganizationDto() };
        }

        public object Put(OrganizationDto organizationDto)
        {
            var organization = organizationDto.ToEntity();
            Db.Update(organization);
            return new OrganizationResponse { Organization = new OrganizationDto() };
        }

        public object Delete(OrganizationDto organizationDto)
        {
            Db.DeleteById<Organization>(organizationDto.Id);
            return new OrganizationResponse { Organization = new OrganizationDto() };
        }
    }

    /// <summary>
    /// GET /organizations
    /// GET /organizations/category/{CategoryId}
    /// Returns a list of organizations
    /// </summary>
    public class OrganizationsService : ServiceStack.ServiceInterface.Service
    {
        public object Get(OrganizationsDto request)
        {
            var orgs = request.CategoryId.HasValue
                           ? Db.Select<OrganizationDto>()
                               .Where(o => o.Categories.Any(c => c.Id == request.CategoryId.Value))
                               .ToList()
                           : Db.Select<OrganizationDto>("select * from Organization");

            return new OrganizationsResponse
                       {
                           Organizations = orgs
                       };
        }
    }
}