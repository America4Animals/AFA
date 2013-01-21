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

        #region Events

        public static EventModel ToModel(this EventDto entity)
        {
            return Mapper.Map<EventDto, EventModel>(entity);
        }

        public static EventDto ToEntity(this EventModel model)
        {
            return Mapper.Map<EventModel, EventDto>(model);
        }

        public static EventDetailModel ToDetailModel(this EventDto entity)
        {
            var model = Mapper.Map<EventDto, EventDetailModel>(entity);
            model.StartHour = model.StartDateTime.Hour;
            model.StartMinute = model.StartDateTime.Minute;
            return model;
        }

        public static EventDto ToEntity(this EventDetailModel model)
        {
            return Mapper.Map<EventDetailModel, EventDto>(model);
        }

        #endregion
    }
}