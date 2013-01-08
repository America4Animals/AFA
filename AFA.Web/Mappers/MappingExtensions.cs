using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AFA.Web.Models;
using AutoMapper;

namespace AFA.Web.Mappers
{
    public static class MappingExtensions
    {
        #region Organizations

        public static OrganizationModel ToModel(this OrganizationDto entity)
        {
            return Mapper.Map<OrganizationDto, OrganizationModel>(entity);
        }

        public static OrganizationDto ToEntity(this OrganizationModel model)
        {
            return Mapper.Map<OrganizationModel, OrganizationDto>(model);
        }

        public static OrganizationDto ToEntity(this OrganizationModel model, OrganizationDto destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion
    }
}