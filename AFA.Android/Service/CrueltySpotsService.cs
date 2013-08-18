using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Utility;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Parse;
using ServiceStack.Text;

namespace AFA.Android.Service
{
    public class CrueltySpotsService
    {
        public const string NameField = "name";
        public const string CityField = "city";
        public const string StateProvinceAbbreviationField = "stateProvinceAbbreviation";

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

            var results = await query.FindAsync();

            switch (sortField)
            {
                case CrueltySpotSortField.CreatedAt:
                    results = sortDirection == SortDirection.Asc ? 
                        results.OrderBy(r => r.CreatedAt) : results.OrderByDescending(r => r.CreatedAt);
                    break;
            }

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
    }
}