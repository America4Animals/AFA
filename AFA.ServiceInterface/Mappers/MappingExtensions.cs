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

        #region Users

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

        public static Event ToEntity(this EventDto dto)
        {
            return Mapper.Map<EventDto, Event>(dto);
        }

        public static EventDto ToDto(this Event entity)
        {
            return Mapper.Map<Event, EventDto>(entity);
        }

        #endregion

        #region Cruelty Spots

        public static CrueltySpot ToEntity(this CrueltySpotDto dto)
        {
            return Mapper.Map<CrueltySpotDto, CrueltySpot>(dto);
        }

        public static CrueltySpotDto ToDto(this CrueltySpot entity)
        {
            return Mapper.Map<CrueltySpot, CrueltySpotDto>(entity);
        }

        #endregion
    }
}
