using System;
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
            var query = string.Format("select o.*, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation, oc.Id as OrganizationCategoryId, oc.Name as OrganizationCategoryName " +
                                     "from Organization o " +
                                     "left join StateProvince sp " +
                                     "on o.StateProvinceId = sp.Id " +
                                     "left join OrganizationCategory oc " +
                                     "on o.OrganizationCategoryId = oc.Id " +
                                     "where o.Id = {0}", organizationDto.Id);

            var orgDto = Db.Select<OrganizationDto>(query).FirstOrDefault();

            if (orgDto != null)
            {
                //// Categories
                //var orgCategoryQuery = string.Format(
                //    "select oc.* " +
                //    "from OrganizationCategory oc " +
                //    "inner join OrganizationOrganizationCategory ooc " +
                //    "on oc.Id = ooc.OrganizationCategoryId " +
                //    "where ooc.OrganizationId = {0}", orgDto.Id);

                //var orgCategories = Db.Select<OrganizationCategory>(orgCategoryQuery);

                //if (orgCategories != null)
                //{
                //    orgDto.Categories = orgCategories;
                //}

                // Allies
                var alliesCountQuery = string.Format("select count(*) from OrganizationAlly where OrganizationId = {0}", orgDto.Id);
                orgDto.OrganizationAlliesCount = Db.Scalar<int>(alliesCountQuery);

                // News
                var newsCountQuery = string.Format("select count(*) from OrganizationNews where OrganizationId = {0}", orgDto.Id);
                orgDto.OrganizationNewsCount = Db.Scalar<int>(newsCountQuery);

                // Comments
                var commentsCountQuery = string.Format("select count(*) from OrganizationComment where OrganizationId = {0}", orgDto.Id);
                orgDto.OrganizationNewsCount = Db.Scalar<int>(commentsCountQuery);
            }

            return new OrganizationResponse {Organization = orgDto};
        }

        public object Post(OrganizationDto organizationDto)
        {
            var organization = organizationDto.ToEntity();
            Db.Insert(organization);
            var newOrgId = (int)Db.GetLastInsertId();

            //Db.Delete<OrganizationOrganizationCategory>(ooc => ooc.OrganizationId == newOrgId);

            //if (organizationDto.Categories != null && organizationDto.Categories.Any())
            //{
            //    Db.InsertAll(organizationDto.Categories.Select(c => new OrganizationOrganizationCategory
            //                                                            {
            //                                                                OrganizationCategoryId = c.Id,
            //                                                                OrganizationId = newOrgId
            //                                                            }));
            //}

            return new OrganizationResponse { Organization = new OrganizationDto() };
        }

        public object Put(OrganizationDto organizationDto)
        {
            var organization = organizationDto.ToEntity();
            Db.Update(organization);

            //Db.Delete<OrganizationOrganizationCategory>(ooc => ooc.OrganizationId == organizationDto.Id);

            //if (organizationDto.Categories != null && organizationDto.Categories.Any())
            //{
            //    Db.InsertAll(organizationDto.Categories.Select(c => new OrganizationOrganizationCategory
            //    {
            //        OrganizationCategoryId = c.Id,
            //        OrganizationId = organizationDto.Id
            //    }));
            //}

            return new OrganizationResponse { Organization = new OrganizationDto() };
        }

        public object Delete(OrganizationDto organizationDto)
        {
            Db.DeleteById<Organization>(organizationDto.Id);
            //Db.Delete<OrganizationOrganizationCategory>(ooc => ooc.OrganizationId == organizationDto.Id);

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
            var orgs = new List<OrganizationDto>();

            if (request.CategoryId.HasValue)
            {
                //var query = string.Format("select o.*, oc.Id as OrganizationCategoryId, oc.Name as OrganizationCategoryName " +
                //                     "from Organization o " +
                //                     "left join OrganizationCategory oc " +
                //                     "on o.OrganizationCategoryId = oc.Id " +
                //                     "where o.OrganizationCategoryId = {0}", request.CategoryId);

                //orgs = Db.Select<OrganizationDto>("select * from Organization");
            }
            else
            {
                orgs = Db.Select<OrganizationDto>("select * from Organization");    
            }

            foreach (var org in orgs)
            {
                var query = string.Format("select count(*) from OrganizationAlly where OrganizationId = {0}", org.Id);
                org.OrganizationAlliesCount = Db.Scalar<int>(query);
            }

            return new OrganizationsResponse
                       {
                           Organizations = orgs
                       };
        }
    }
}