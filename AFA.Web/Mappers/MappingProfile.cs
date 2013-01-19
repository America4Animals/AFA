using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AFA.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFA.Web.Mappers
{
    public class MappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "MappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<OrganizationDto, OrganizationModel>();
            Mapper.CreateMap<OrganizationModel, OrganizationDto>();

            Mapper.CreateMap<OrganizationDto, OrganizationDetailModel>();
            Mapper.CreateMap<OrganizationDetailModel, OrganizationDto>();

            Mapper.CreateMap<UserDto, UserModel>();
            Mapper.CreateMap<UserModel, UserDto>();

            Mapper.CreateMap<UserDto, UserDetailModel>();
            Mapper.CreateMap<UserDetailModel, UserDto>();
        }
    }
}