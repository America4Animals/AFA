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
                    crueltySpot.StateProvinceId = stateProvince.Id;
                }
            }

            crueltySpot.CreatedAt = DateTime.UtcNow;
            crueltySpot.LastUpdatedAt = DateTime.UtcNow;
            Db.Insert(crueltySpot);
            var crueltySpotId = Db.GetLastInsertId();
            return new CrueltySpotResponse { CrueltySpot = new CrueltySpotDto { Id = Convert.ToInt32(crueltySpotId) } };
        }

        public object Put(CrueltySpotDto crueltySpotDto)
        {
            var crueltySpot = crueltySpotDto.ToEntity();
            crueltySpot.LastUpdatedAt = DateTime.UtcNow;
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
            string query =
                "select cs.*, sp.Name as StateProvinceName, sp.Abbreviation as StateProvinceAbbreviation, csc.Id as CrueltySpotCategoryId, csc.Name as CrueltySpotCategoryName " +
                "from CrueltySpot cs " +
                "left join StateProvince sp " +
                "on cs.StateProvinceId = sp.Id " +
                "left join CrueltySpotCategory csc " +
                "on cs.CrueltySpotCategoryId = csc.Id";

            const string WhereClausePrefix = " where ";
            string whereClause = WhereClausePrefix;

            if (request.CategoryId.HasValue)
            {
                whereClause += String.Format("csc.Id = {0}", request.CategoryId);
            }

            if (!String.IsNullOrEmpty(request.Name))
            {
                if (whereClause != WhereClausePrefix)
                {
                    whereClause += " and ";
                }

                whereClause += String.Format("cs.Name = '{0}'", request.Name);
            }

            if (!String.IsNullOrEmpty(request.City))
            {
                if (whereClause != WhereClausePrefix)
                {
                    whereClause += " and ";
                }

                whereClause += String.Format("cs.City = '{0}'", request.City);
            }

            if (!String.IsNullOrEmpty(request.StateProvinceAbbreviation))
            {
                if (whereClause != WhereClausePrefix)
                {
                    whereClause += " and ";
                }

                whereClause += String.Format("sp.Abbreviation = '{0}'", request.StateProvinceAbbreviation);
            }

            if (whereClause != WhereClausePrefix)
            {
                query += whereClause;
            }

            if (!String.IsNullOrEmpty(request.SortBy))
            {
                query += String.Format(" order by {0}", request.SortBy);

                if (request.SortOrder != null && request.SortOrder.ToLower() == "desc")
                {
                    query += " desc";
                }
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
