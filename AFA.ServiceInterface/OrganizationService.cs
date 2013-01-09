using System.Collections.Generic;
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
            //var query = string.Format("select o.*, oc.Id as OrganizationCategoryId, oc.Name as OrganizationCategoryName, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation " +
            //                          "from Organization o " +
            //                          "left join OrganizationOrganizationCategory ooc " +
            //                          "on o.Id = ooc.OrganizationId " +
            //                          "left join OrganizationCategory oc " +
            //                          "on ooc.OrganizationCategoryId = oc.Id " +
            //                          "left join StateProvince sp " +
            //                          "on o.StateProvinceId = sp.Id " +
            //                          "where o.Id = {0}", organizationDto.Id);

            //return new OrganizationResponse
            //{
            //    Organization = Db.Select<OrganizationDto>(query).FirstOrDefault()
            //};

            var query = string.Format("select o.*, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation " +
                                     "from Organization o " +
                                     "left join StateProvince sp " +
                                     "on o.StateProvinceId = sp.Id " +
                                     "where o.Id = {0}", organizationDto.Id);

            var orgDto = Db.Select<OrganizationDto>(query).FirstOrDefault();

            if (orgDto != null)
            {
                var orgCategoryQuery = string.Format(
                    "select oc.* " +
                    "from OrganizationCategory oc " +
                    "inner join OrganizationOrganizationCategory ooc " +
                    "on oc.Id = ooc.OrganizationCategoryId " +
                    "where ooc.OrganizationId = {0}", orgDto.Id);

                var orgCategories = Db.Select<OrganizationCategory>(orgCategoryQuery);

                if (orgCategories != null)
                {
                    orgDto.Categories = orgCategories;
                }
            }

            return new OrganizationResponse {Organization = orgDto};
        }

        public object Post(OrganizationDto organizationDto)
        {
            var organization = organizationDto.ToEntity();
            Db.Insert(organization);
            var newOrgId = (int)Db.GetLastInsertId();

            Db.Delete<OrganizationOrganizationCategory>(ooc => ooc.OrganizationId == newOrgId);

            if (organizationDto.Categories != null && organizationDto.Categories.Any())
            {
                Db.InsertAll(organizationDto.Categories.Select(c => new OrganizationOrganizationCategory
                                                                        {
                                                                            OrganizationCategoryId = c.Id,
                                                                            OrganizationId = newOrgId
                                                                        }));
            }

            return new OrganizationResponse { Organization = new OrganizationDto() };
        }

        public object Put(OrganizationDto organizationDto)
        {
            var organization = organizationDto.ToEntity();
            Db.Update(organization);

            Db.Delete<OrganizationOrganizationCategory>(ooc => ooc.OrganizationId == organizationDto.Id);

            if (organizationDto.Categories != null && organizationDto.Categories.Any())
            {
                Db.InsertAll(organizationDto.Categories.Select(c => new OrganizationOrganizationCategory
                {
                    OrganizationCategoryId = c.Id,
                    OrganizationId = organizationDto.Id
                }));
            }

            return new OrganizationResponse { Organization = new OrganizationDto() };
        }

        public object Delete(OrganizationDto organizationDto)
        {
            Db.DeleteById<Organization>(organizationDto.Id);
            Db.Delete<OrganizationOrganizationCategory>(ooc => ooc.OrganizationId == organizationDto.Id);

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