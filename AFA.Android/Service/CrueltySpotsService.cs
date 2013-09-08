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
//using ServiceStack.Text;

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

            query = ApplySort(query, sortField, sortDirection);

            var results = await query.FindAsync();

            //switch (sortField)
            //{
            //    case CrueltySpotSortField.CreatedAt:
            //        results = sortDirection == SortDirection.Asc ? 
            //            results.OrderBy(r => r.CreatedAt) : results.OrderByDescending(r => r.CreatedAt);
            //        break;
            //}

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
            var categoriesQuery = from crueltySpotCategory in new ParseQuery<CrueltySpotCategory>()
                                  where crueltySpotCategoryIds.Contains(crueltySpotCategory.ObjectId)
                                  select crueltySpotCategory;

            var query = new ParseQuery<CrueltySpot>()
                .WhereMatchesQuery(CrueltySpotCategoryFieldName, categoriesQuery);

            if (retrieveCategoryTypes)
            {
                query = query.Include(CrueltySpotCategoryFieldName);
            }

            query = ApplySort(query, sortField, sortDirection);

            var results = await query.FindAsync();
            return results.ToList();
        }

        public async Task<List<CrueltySpot>> GetManyAsync(double latitude, double longititude, int distanceInMiles, bool retrieveCategoryTypes, 
            CrueltySpotSortField sortField,
            SortDirection sortDirection)
		{
			var requestGeoPoint = new ParseGeoPoint (latitude, longititude);
			var query = new ParseQuery<CrueltySpot> ();
			query = query.WhereWithinDistance(LocationField, requestGeoPoint, ParseGeoDistance.FromMiles(distanceInMiles));

			if (retrieveCategoryTypes)
			{
				query = query.Include(CrueltySpotCategoryFieldName);
			}

            query = ApplySort(query, sortField, sortDirection);

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