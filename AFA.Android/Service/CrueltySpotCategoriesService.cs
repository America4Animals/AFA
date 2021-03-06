using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Parse;
//using ServiceStack.Text;
using Android.Util;

namespace AFA.Android.Service
{
    public class CrueltySpotCategoriesService
    {
        public async Task<List<CrueltySpotCategory>> GetAllAsync()
        {
            var query = new ParseQuery<CrueltySpotCategory>();
            var result = await query.FindAsync();
            return result.ToList();
        }

        public async Task<CrueltySpotCategory> GetByIdAsync(string id)
        {
            //var query = ParseObject.GetQuery(ParseHelper.CrueltySpotCategoryClassName);
            //var crueltySpotCategoryParse = await query.GetAsync(id);
            //return ConvertToPoco(crueltySpotCategoryParse);

            var query = new ParseQuery<CrueltySpotCategory>();
            var result = await query.GetAsync(id);
            return result;
        }

        //public async Task<ParseObject> GetParseObjectByIdAsync(string id)
        //{
        //    var query = ParseObject.GetQuery(ParseHelper.CrueltySpotCategoryClassName);
        //    return await query.GetAsync(id);
        //}

        //public CrueltySpotCategory ConvertToPoco(ParseObject crueltySpotCategoryParse)
        //{
        //    var crueltySpotCategory = new CrueltySpotCategory();
        //    //crueltySpotCategory.Id = crueltySpotCategoryParse.ObjectId;
        //    string name;
        //    string description;
        //    string iconName;
        //    crueltySpotCategoryParse.TryGetValue("name", out name);
        //    crueltySpotCategoryParse.TryGetValue("description", out description);
        //    crueltySpotCategoryParse.TryGetValue("iconName", out iconName);
        //    crueltySpotCategory.Name = name;
        //    crueltySpotCategory.Description = description;
        //    crueltySpotCategory.IconName = iconName;
        //    return crueltySpotCategory;
        //}
    }
}