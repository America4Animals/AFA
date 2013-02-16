using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceHostAndWeb.Models;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AutoMapper;

namespace AFA.ServiceHostAndWeb.Mappers
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

        // Domain
        public static Organization ToEntity(this OrganizationDto dto)
        {
            return Mapper.Map<OrganizationDto, Organization>(dto);
        }

        public static OrganizationDto ToDto(this Organization entity)
        {
            return Mapper.Map<Organization, OrganizationDto>(entity);
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

        // Domain
        public static User ToEntity(this UserDto dto)
        {
            return Mapper.Map<UserDto, User>(dto);
        }

        public static UserDto ToDto(this User entity)
        {
            return Mapper.Map<User, UserDto>(entity);
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

        // Domain
        public static Event ToEntity(this EventDto dto)
        {
            return Mapper.Map<EventDto, Event>(dto);
        }

        public static EventDto ToDto(this Event entity)
        {
            return Mapper.Map<Event, EventDto>(entity);
        }

        #endregion
    }
}