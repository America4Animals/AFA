using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Utility;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Parse;
using Newtonsoft.Json;

namespace AFA.Android.Service
{
    public class CrueltySpotsService
    {
        public const string NameField = "name";
        public const string CityField = "city";
        public const string StateProvinceAbbreviationField = "stateProvinceAbbreviation";
		public const string LocationField = "location";

        public const string CrueltySpotCategoryFieldName = "crueltySpotCategory";


        public async Task<CrueltySpot> GetByIdAsync(string id, bool includeCategory)
        {
            var query = new ParseQuery<CrueltySpot>();

            if (includeCategory)
            {
                query = query.Include(CrueltySpotCategoryFieldName);
            }

            var result = await query.GetAsync(id);
            return result;
        }

        /// <summary>
        /// Get many cruelty spots matching the criteria specified in the request parameter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="retrieveCategoryTypes"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public async Task<List<CrueltySpot>> GetManyAsync(CrueltySpot request, bool retrieveCategoryTypes,
            CrueltySpotSortField sortField, SortDirection sortDirection)
        {
            var query = new ParseQuery<CrueltySpot>();
            if (retrieveCategoryTypes)
            {
                query = query.Include(CrueltySpotCategoryFieldName);
            }

            if (!String.IsNullOrEmpty(request.Name))
            {
                query = query.WhereEqualTo(NameField, request.Name);
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                query = query.WhereEqualTo(CityField, request.City);
            }

            if (!string.IsNullOrEmpty(request.StateProvinceAbbreviation))
            {
                query = query.WhereEqualTo(StateProvinceAbbreviationField, request.StateProvinceAbbreviation);
            }

			DebugHelper.WriteDebugEntry ("Applying sort to CrueltySpotQuery: " + query.ToString ());
			DebugHelper.WriteDebugEntry ("Sort Field: " + sortField.ToString());
			DebugHelper.WriteDebugEntry ("Sort Dir: " + sortDirection.ToString());
			//query = ApplySort(query, sortField, sortDirection);
			DebugHelper.WriteDebugEntry ("Applied sort to query, fetching results");
            var results = await query.FindAsync();

            return results.ToList();
        }

        /// <summary>
        /// Get many cruelty spots matching the specified category IDs
        /// </summary>
        /// <param name="crueltySpotCategoryIds"></param>
        /// <param name="retrieveCategoryTypes"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public async Task<List<CrueltySpot>> GetManyAsync(
            IEnumerable<string> crueltySpotCategoryIds,
            bool retrieveCategoryTypes, 
            CrueltySpotSortField sortField, 
            SortDirection sortDirection)
        {
            //var categoriesQuery = from crueltySpotCategory in new ParseQuery<CrueltySpotCategory>()
            //                      where crueltySpotCategoryIds.Contains(crueltySpotCategory.ObjectId)
            //                      select crueltySpotCategory;

            //var query = new ParseQuery<CrueltySpot>()
            //    .WhereMatchesQuery(CrueltySpotCategoryFieldName, categoriesQuery);

            //if (retrieveCategoryTypes)
            //{
            //    query = query.Include(CrueltySpotCategoryFieldName);
            //}

            //query = ApplySort(query, sortField, sortDirection);

            //var results = await query.FindAsync();
            //return results.ToList();
			DebugHelper.WriteDebugEntry ("Getting cruelty spots for category IDs: " + JsonConvert.SerializeObject(crueltySpotCategoryIds));
			return await GetManyAsync(null, crueltySpotCategoryIds.ToList(), retrieveCategoryTypes, sortField, sortDirection);
        }

        public async Task<List<CrueltySpot>> GetManyAsync(
            GeoQueryRequest geoQueryRequest, 
            bool retrieveCategoryTypes, 
            CrueltySpotSortField sortField,
            SortDirection sortDirection)
		{
            //var requestGeoPoint = new ParseGeoPoint(geoQueryRequest.Latitude, geoQueryRequest.Longititude);
            //var query = new ParseQuery<CrueltySpot> ();
            //query = query.WhereWithinDistance(LocationField, requestGeoPoint, ParseGeoDistance.FromMiles(geoQueryRequest.DistanceInMiles));

            //if (retrieveCategoryTypes)
            //{
            //    query = query.Include(CrueltySpotCategoryFieldName);
            //}

            //query = ApplySort(query, sortField, sortDirection);

            //var results = await query.FindAsync();
            //return results.ToList();

            return await GetManyAsync(geoQueryRequest, null, retrieveCategoryTypes, sortField, sortDirection);
		}

        public async Task<List<CrueltySpot>> GetManyAsync(
            GeoQueryRequest geoQueryRequest,
			List<string> crueltySpotCategoryIds,
            bool retrieveCategoryTypes,
            CrueltySpotSortField sortField,
            SortDirection sortDirection
            )
        {
            var query = new ParseQuery<CrueltySpot>();

            if (geoQueryRequest != null)
            {
                var requestGeoPoint = new ParseGeoPoint(geoQueryRequest.Latitude, geoQueryRequest.Longititude);
                query = query.WhereWithinDistance(LocationField, requestGeoPoint, ParseGeoDistance.FromMiles(geoQueryRequest.DistanceInMiles));
            }

            if (crueltySpotCategoryIds != null)
            {
				DebugHelper.WriteDebugEntry ("Begin apply category filter for cruelty spots for category IDs: " + JsonConvert.SerializeObject(crueltySpotCategoryIds));
				var categoriesQuery = from crueltySpotCategory in new ParseQuery<CrueltySpotCategory>()
                                      where crueltySpotCategoryIds.Contains(crueltySpotCategory.ObjectId)
                                      select crueltySpotCategory;

                query = query.WhereMatchesQuery(CrueltySpotCategoryFieldName, categoriesQuery);

				/*var categoriesQuery = from crueltySpotCategory in new ParseQuery<CrueltySpotCategory>()
					                      where crueltySpotCategoryIds.Contains(crueltySpotCategory.Get<string>("objectId"))
				                      select crueltySpotCategory;

				query = query.WhereMatchesQuery(CrueltySpotCategoryFieldName, categoriesQuery);*/
				DebugHelper.WriteDebugEntry ("End apply category filter");
            }

            if (retrieveCategoryTypes)
            {
                query = query.Include(CrueltySpotCategoryFieldName);
            }

			DebugHelper.WriteDebugEntry ("Applying sort to GetManyAsync Cruelty Spots query");
            query = ApplySort(query, sortField, sortDirection);
			DebugHelper.WriteDebugEntry ("Running query: " + query.ToString());
            var results = await query.FindAsync();
            return results.ToList();
        }

        public async Task<List<CrueltySpot>> GetAllAsync(bool retrieveCategoryTypes)
        {
            var query = new ParseQuery<CrueltySpot>();

            if (retrieveCategoryTypes)
            {
                query = query.Include(CrueltySpotCategoryFieldName);
            }

            var result = await query.FindAsync();
            return result.ToList();
        }

        public async Task<List<CrueltySpot>> GetAllGooglePlacesAsync()
        {
            var query = from crueltySpot in new ParseQuery<CrueltySpot>()
                        where crueltySpot.GooglePlaceId != null
                        select crueltySpot;

            var result = await query.FindAsync();
            return result.ToList();
        }

        public async Task<string> SaveAsync(CrueltySpot crueltySpot)
        {
            crueltySpot.Location = new ParseGeoPoint(crueltySpot.Latitude, crueltySpot.Longitude);
            await crueltySpot.SaveAsync();
            return crueltySpot.ObjectId;
        }

        private ParseQuery<T> ApplySort<T>(ParseQuery<T> query, CrueltySpotSortField sortField, SortDirection sortDirection) where T : ParseObject
        {
            switch (sortField)
            {
                case CrueltySpotSortField.CreatedAt:
                    query = sortDirection == SortDirection.Asc ?
                        query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt);
                    break;
            }

            return query;
        }
    }
}