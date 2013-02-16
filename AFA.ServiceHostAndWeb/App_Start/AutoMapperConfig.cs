using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceHostAndWeb.Mappers;
using AutoMapper;

namespace AFA.ServiceHostAndWeb.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());
        }
    }
}