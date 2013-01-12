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

        public static OrganizationDetailModel ToDetailModel(this OrganizationDto entity)
        {
            return Mapper.Map<OrganizationDto, OrganizationDetailModel>(entity);
        }

        public static OrganizationDto ToEntity(this OrganizationDetailModel model)
        {
            return Mapper.Map<OrganizationDetailModel, OrganizationDto>(model);
        }

        #endregion
    }
}