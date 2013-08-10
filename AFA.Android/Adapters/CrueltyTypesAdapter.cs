using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Library.ServiceModel;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AFA.ServiceModel;

namespace AFA.Android
{
    class CrueltyTypesAdapter : BaseAdapter<CrueltySpotCategory>
    {
        private readonly Activity _context;
        private readonly IList<CrueltySpotCategory> _crueltySpotCategories;


        public CrueltyTypesAdapter(Activity context, IList<CrueltySpotCategory> crueltySpotCategories)
        {
            _context = context;
            _crueltySpotCategories = crueltySpotCategories;
        }

        public override CrueltySpotCategory this[int position]
        {
            get { return _crueltySpotCategories[position]; }
        }

        public override int Count
        {
            get { return _crueltySpotCategories.Count; }
        }

        public override long GetItemId(int position)
        {
            //return _crueltySpotCategorieses[position].Id;
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CrueltyTypeListItem, null);

            var crueltySpotCategory = _crueltySpotCategories[position];
            view.FindViewById<TextView>(Resource.Id.Name).Text = crueltySpotCategory.Name;
            var resourceId = _context.Resources.GetIdentifier(crueltySpotCategory.IconName.Replace(".png", ""), "drawable",
                                                              _context.PackageName);
            view.FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
            return view;
        }
    }
}