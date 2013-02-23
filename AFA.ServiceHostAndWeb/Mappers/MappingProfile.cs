using AFA.ServiceHostAndWeb.Models;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFA.ServiceHostAndWeb.Mappers
{
    public class MappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "MappingProfile"; }
        }

        protected override void Configure()
        {
            #region Organizations

            // View Model mapping
            Mapper.CreateMap<OrganizationDto, OrganizationModel>();
            Mapper.CreateMap<OrganizationModel, OrganizationDto>();

            Mapper.CreateMap<OrganizationDto, OrganizationDetailModel>();
            Mapper.CreateMap<OrganizationDetailModel, OrganizationDto>();

            // Domain mapping
            Mapper.CreateMap<OrganizationDto, Organization>();
            Mapper.CreateMap<Organization, OrganizationDto>();

            #endregion

            #region Users

            // View Model mapping
            Mapper.CreateMap<UserDto, UserModel>();
            Mapper.CreateMap<UserModel, UserDto>();

            Mapper.CreateMap<UserDto, UserDetailModel>();
            Mapper.CreateMap<UserDetailModel, UserDto>();

            // Domain mapping
            Mapper.CreateMap<UserDto, User>();
            Mapper.CreateMap<User, UserDto>();

            #endregion

            #region Events

            // View Model mapping
            Mapper.CreateMap<EventDto, EventModel>();
            Mapper.CreateMap<EventModel, EventDto>();

            Mapper.CreateMap<EventDto, EventDetailModel>();
            Mapper.CreateMap<EventDetailModel, EventDto>();

            // Domain mapping
            Mapper.CreateMap<EventDto, Event>();
            Mapper.CreateMap<Event, EventDto>();

            #endregion

            #region CrueltySpots

            // View Model mapping
            Mapper.CreateMap<CrueltySpotDto, CrueltySpotModel>();
            Mapper.CreateMap<CrueltySpotModel, CrueltySpotDto>();

            Mapper.CreateMap<CrueltySpotDto, CrueltySpotDetailModel>();
            Mapper.CreateMap<CrueltySpotDetailModel, CrueltySpotDto>();

            // Domain mapping
            Mapper.CreateMap<CrueltySpotDto, CrueltySpot>();
            Mapper.CreateMap<CrueltySpot, CrueltySpotDto>();

            #endregion
        }
    }
}