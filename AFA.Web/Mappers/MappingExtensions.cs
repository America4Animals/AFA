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

        #region Users

        public static UserModel ToModel(this UserDto entity)
        {
            return Mapper.Map<UserDto, UserModel>(entity);
        }

        public static UserDto ToEntity(this UserModel model)
        {
            return Mapper.Map<UserModel, UserDto>(model);
        }

        public static UserDetailModel ToDetailModel(this UserDto entity)
        {
            return Mapper.Map<UserDto, UserDetailModel>(entity);
        }

        public static UserDto ToEntity(this UserDetailModel model)
        {
            return Mapper.Map<UserDetailModel, UserDto>(model);
        }

        #endregion
    }
}