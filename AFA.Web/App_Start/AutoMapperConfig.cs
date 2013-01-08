using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.Web.Mappers;
using AutoMapper;

namespace AFA.Web.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());
        }
    }
}