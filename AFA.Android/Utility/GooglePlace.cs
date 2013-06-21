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

namespace AFA.Android.Helpers
{
    public class GooglePlace
    {
        public string Name { get; set; }
        public string Vicinity { get; set; }
        public string Reference { get; set; }
    }
}