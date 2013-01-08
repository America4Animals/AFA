using AFA.ServiceModel;
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
            Mapper.CreateMap<Organization, OrganizationModel>();
            Mapper.CreateMap<OrganizationModel, Organization>();
        }
    }
}