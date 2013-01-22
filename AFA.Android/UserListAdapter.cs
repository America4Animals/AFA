using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android
{
    public class UserListAdapter : BaseAdapter<UserDto>
    {
        private readonly Activity _context;
        private readonly IList<UserDto> _users;

        public UserListAdapter(Activity context, IList<UserDto> users)
        {
            _context = context;
            _users = users;
        }

        public override UserDto this[int position]
        {
            get { return _users[position]; }
        }

        public override int Count
        {
            get { return _users.Count; }
        }

        public override long GetItemId(int position)
        {
            return _users[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.UserListItem, null);
            var user = _users[position];
            view.FindViewById<TextView>(Resource.Id.UserName).Text = user.DisplayName;
            view.FindViewById<TextView>(Resource.Id.CityState).Text = user.CityAndState;
            return view;
        }
    }
}