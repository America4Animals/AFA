using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceInterface.Mappers;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    public class CrueltySpotService : ServiceStack.ServiceInterface.Service, ICrueltySpotService
    {
        /// <summary>
        /// GET /crueltyspots/{Id}
        /// </summary>
        public CrueltySpotResponse Get(CrueltySpotDto request)
        {
            var query = string.Format("select cs.*, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation, csc.Id as CrueltySpotCategoryId, csc.Name as CrueltySpotCategoryName " +
                                     "from CrueltySpot cs " +
                                     "left join StateProvince sp " +
                                     "on cs.StateProvinceId = sp.Id " +
                                     "left join CrueltySpotCategory csc " +
                                     "on cs.CrueltySpotCategoryId = csc.Id " +
                                     "where cs.Id = {0}", request.Id);

            var crueltySpotDto = Db.Select<CrueltySpotDto>(query).FirstOrDefault();
            return new CrueltySpotResponse { CrueltySpot = crueltySpotDto };
        }

        public object Post(CrueltySpotDto request)
        {
            var crueltySpot = request.ToEntity();

            if (request.StateProvinceId < 1 && !String.IsNullOrEmpty(request.StateProvinceAbbreviation))
            {
                var stateQuery = String.Format("select * " +
                                               "from StateProvince " +
                                               "where Abbreviation = '{0}'", request.StateProvinceAbbreviation);

                var stateProvince = Db.Select<StateProvince>(stateQuery).FirstOrDefault();
                if (stateProvince != null)
                {
                    request.StateProvinceId = stateProvince.Id;
                }
            }

            Db.Insert(crueltySpot);
            var crueltySpotId = Db.GetLastInsertId();
            return new CrueltySpotResponse { CrueltySpot = new CrueltySpotDto { Id = Convert.ToInt32(crueltySpotId) } };
        }

        public object Put(CrueltySpotDto crueltySpotDto)
        {
            var crueltySpot = crueltySpotDto.ToEntity();
            Db.Update(crueltySpot);
            return new CrueltySpotResponse { CrueltySpot = new CrueltySpotDto() };
        }

        public object Delete(CrueltySpotDto crueltySpotDto)
        {
            Db.DeleteById<CrueltySpot>(crueltySpotDto.Id);
            return new CrueltySpotResponse { CrueltySpot = new CrueltySpotDto() };
        }
    }

    /// <summary>
    /// GET /crueltyspots
    /// GET /crueltyspots/category/{CategoryId}
    /// Returns a list of Cruelty Spots
    /// </summary>
    public class CrueltySpotsService : ServiceStack.ServiceInterface.Service, ICrueltySpotsService
    {
        public CrueltySpotsResponse Get(CrueltySpotsDto request)
        {
            var crueltySpots = new List<CrueltySpotDto>();
            string query;

            if (request.CategoryId.HasValue)
            {
                throw new NotImplementedException();
            }
            else if (!String.IsNullOrEmpty(request.Name) && !String.IsNullOrEmpty(request.City) &&
                     !String.IsNullOrEmpty(request.StateProvinceAbbreviation))
            {
                query =
                    string.Format(
                        "select cs.*, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation, csc.Id as CrueltySpotCategoryId, csc.Name as CrueltySpotCategoryName " +
                        "from CrueltySpot cs " +
                        "left join StateProvince sp " +
                        "on cs.StateProvinceId = sp.Id " +
                        "left join CrueltySpotCategory csc " +
                        "on cs.CrueltySpotCategoryId = csc.Id " +
                        "where cs.Name = '{0}' and cs.City = '{1}' and sp.Abbreviation = '{2}'", request.Name, request.City, request.StateProvinceAbbreviation);
            }
            else
            {
                query =
                    string.Format(
                        "select cs.*, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation, csc.Id as CrueltySpotCategoryId, csc.Name as CrueltySpotCategoryName " +
                        "from CrueltySpot cs " +
                        "left join StateProvince sp " +
                        "on cs.StateProvinceId = sp.Id " +
                        "left join CrueltySpotCategory csc " +
                        "on cs.CrueltySpotCategoryId = csc.Id");
            }

            crueltySpots = Db.Select<CrueltySpotDto>(query);

            return new CrueltySpotsResponse
            {
                CrueltySpots = crueltySpots
            };
        }

        /// <summary>
        /// GET /crueltyspots/googleplaces
        /// Returns cruelty spots google place ids (only)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CrueltySpotsGooglePlacesResponse Get(CrueltySpotsGooglePlaces request)
        {
            var crueltySpots = new List<CrueltySpotDto>();
            string query = string.Format("select Id, GooglePlaceId " +
                                     "from CrueltySpot cs " +
                                     "where GooglePlaceId is not null");

            crueltySpots = Db.Select<CrueltySpotDto>(query);

            return new CrueltySpotsGooglePlacesResponse
            {
                CrueltySpots = crueltySpots
            };
        }
    }
}
