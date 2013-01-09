using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android
{
    public class OrganizationListAdapter : BaseAdapter<OrganizationDto>
    {
        private readonly Activity _context;
        private readonly IList<OrganizationDto> _organizations;

        public OrganizationListAdapter(Activity context, IList<OrganizationDto> organizations)
        {
            _context = context;
            _organizations = organizations;
        }

        public override OrganizationDto this[int position]
        {
            get { return _organizations[position]; }
        }

        public override int Count
        {
            get { return _organizations.Count; }
        }

        public override long GetItemId(int position)
        {
            return _organizations[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.OrganizationListItem, null);

            var org = _organizations[position];
            view.FindViewById<TextView>(Resource.Id.OrgName).Text = org.Name;
            view.FindViewById<TextView>(Resource.Id.CityState).Text = org.CityAndState;
            var alliesText = string.Format("{0} Allies", org.OrganizationAlliesCount);
            view.FindViewById<TextView>(Resource.Id.Allies).Text = alliesText;
            return view;
        }
    }
}