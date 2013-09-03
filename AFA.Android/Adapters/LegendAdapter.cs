using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Helpers;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace AFA.Android
{
	class LegendAdapter : BaseAdapter<CrueltySpotCategory>
	{
		private readonly Activity _context;
		private readonly IList<CrueltySpotCategory> _crueltySpotCategorieses;


		public LegendAdapter(Activity context, IList<CrueltySpotCategory> crueltySpotCategories)
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
			//return _crueltySpotCategorieses[position].Id;
		    return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.LegendListItem, null);

			var crueltySpotCategory = _crueltySpotCategorieses[position];
			view.FindViewById<TextView>(Resource.Id.Name).Text = crueltySpotCategory.Name;
			var resourceId = _context.Resources.GetIdentifier(crueltySpotCategory.IconName.Replace(".png", ""), "drawable",
			                                                  _context.PackageName);
			view.FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
			resourceId = _context.Resources.GetIdentifier(crueltySpotCategory.IconName.Replace(".png", "pin"), "drawable",
			                                              _context.PackageName);

			view.FindViewById<ImageView>(Resource.Id.CrueltyTypePin).SetImageResource(resourceId);
			CheckBox categoryCheckbox = view.FindViewById<CheckBox>(Resource.Id.categorySelected);
			categoryCheckbox.SetBackgroundColor (Color.Gray);

			List<String> categoryIds = UserPreferencesHelper.GetFilterCategories ();
			// no filter has been set so by default enable the fitler

			if (categoryIds.Contains (crueltySpotCategory.ObjectId)) {
				categoryCheckbox.Checked = true;
			} else {
				categoryCheckbox.Checked = false;
			}

			if (convertView == null) {
				categoryCheckbox.Click += (sender, args) => {


					if (categoryCheckbox.Checked) {
						Toast.MakeText (_context, "selected " + crueltySpotCategory.ObjectId, ToastLength.Short).Show ();
						UserPreferencesHelper.addCategoryFilter (crueltySpotCategory.ObjectId);
					} else {
						Toast.MakeText (_context, "Not selected " + crueltySpotCategory.ObjectId, ToastLength.Short).Show ();
						UserPreferencesHelper.removeCategoryFilter (crueltySpotCategory.ObjectId);
					}

				};
			}

			return view;
		}
	}
}