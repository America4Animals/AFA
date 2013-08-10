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
using Parse;

namespace AFA.Android.Library.ServiceModel
{
    //public class CrueltySpotCategory
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public string IconName { get; set; }
    //}

	[ParseClassName("CrueltySpotCategory")]
	public class CrueltySpotCategory : ParseObject
	{
		[ParseFieldName("name")]
		public string Name
		{
			get { return GetProperty<string>(); }
			set { SetProperty(value); }
		}

		//public string Description { get; set; }

		[ParseFieldName("description")]
		public string Description
		{
			get { return GetProperty<string>(); }
			set { SetProperty(value); }
		}

		[ParseFieldName("iconName")]
		public string IconName
		{
			get { return GetProperty<string>(); }
			set { SetProperty(value); }
		}
	}
}