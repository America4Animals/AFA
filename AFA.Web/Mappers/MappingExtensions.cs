using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceModel;
using AFA.Web.Models;
using AutoMapper;

namespace AFA.Web.Mappers
{
    public static class MappingExtensions
    {
        #region Organizations

        public static OrganizationModel ToModel(this Organization entity)
        {
            return Mapper.Map<Organization, OrganizationModel>(entity);
        }

        public static Organization ToEntity(this OrganizationModel model)
        {
            return Mapper.Map<OrganizationModel, Organization>(model);
        }

        public static Organization ToEntity(this OrganizationModel model, Organization destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion
    }
}