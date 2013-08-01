using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Parse;
using ServiceStack.Text;
using Android.Util;

namespace AFA.Android.Service
{
    public class CrueltySpotCategoriesService
    {
        public async Task<List<CrueltySpotCategory>> GetAllAsync()
        {
            var query = ParseObject.GetQuery(ParseHelper.CrueltySpotCategoryClassName);
            IEnumerable<ParseObject> crueltySpotCategoriesParse = await query.FindAsync();

            var crueltySpotCategories = new List<CrueltySpotCategory>();
            foreach (var crueltySpotCategoryParse in crueltySpotCategoriesParse)
            {
                var crueltySpotCategory = ConvertToPoco(crueltySpotCategoryParse);
                crueltySpotCategories.Add(crueltySpotCategory);
            }

            return crueltySpotCategories;
        }

        public async Task<CrueltySpotCategory> GetByIdAsync(string id)
        {
            var query = ParseObject.GetQuery(ParseHelper.CrueltySpotCategoryClassName);
            var crueltySpotCategoryParse = await query.GetAsync(id);
            return ConvertToPoco(crueltySpotCategoryParse);
        }

        public async Task<ParseObject> GetParseObjectByIdAsync(string id)
        {
            var query = ParseObject.GetQuery(ParseHelper.CrueltySpotCategoryClassName);
            return await query.GetAsync(id);
        }

        public CrueltySpotCategory ConvertToPoco(ParseObject crueltySpotCategoryParse)
        {
            var crueltySpotCategory = new CrueltySpotCategory();
            crueltySpotCategory.Id = crueltySpotCategoryParse.ObjectId;
            string name;
            string description;
            string iconName;
            crueltySpotCategoryParse.TryGetValue("name", out name);
            crueltySpotCategoryParse.TryGetValue("description", out description);
            crueltySpotCategoryParse.TryGetValue("iconName", out iconName);
            crueltySpotCategory.Name = name;
            crueltySpotCategory.Description = description;
            crueltySpotCategory.IconName = iconName;
            return crueltySpotCategory;
        }
    }
}