﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AutoMapper;

namespace AFA.ServiceInterface.Mappers
{
    public class MappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "MappingProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<OrganizationDto, Organization>();
            Mapper.CreateMap<Organization, OrganizationDto>();
        }
    }
}