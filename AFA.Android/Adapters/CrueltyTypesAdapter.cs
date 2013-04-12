using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
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
        private readonly IList<CrueltySpotCategory> _crueltySpotCategorieses;

        public CrueltyTypesAdapter(Activity context, IList<CrueltySpotCategory> crueltySpotCategories)
        {
            _context = context;
            _crueltySpotCategorieses = crueltySpotCategories;
        }

        public override CrueltySpotCategory this[int position]
        {
            get { return _crueltySpotCategorieses[position]; }
        }

        public override int Count
        {
            get { return _crueltySpotCategorieses.Count; }
        }

        public override long GetItemId(int position)
        {
            return _crueltySpotCategorieses[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.PlaceListItem, null);

            var crueltySpotCategory = _crueltySpotCategorieses[position];
            view.FindViewById<TextView>(Resource.Id.Name).Text = crueltySpotCategory.Name;
            return view;
        }
    }
}