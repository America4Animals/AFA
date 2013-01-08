using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AutoMapper;

namespace AFA.ServiceInterface.Mappers
{
    public static class MappingExtensions
    {
        #region Organizations

        public static Organization ToEntity(this OrganizationDto dto)
        {
            return Mapper.Map<OrganizationDto, Organization>(dto);
        }

        public static OrganizationDto ToDto(this Organization entity)
        {
            return Mapper.Map<Organization, OrganizationDto>(entity);
        }

        #endregion
    }
}
