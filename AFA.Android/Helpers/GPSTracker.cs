using System;
using System.Linq;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace AFA_Android.Helpers
{
    public class GPSTracker : Service, ILocationListener
    {
        private Context _context;

        // flag for GPS status
        bool _isGPSEnabled = false;

        // flag for network status
        bool _isNetworkEnabled = false;

        // flag for GPS status
        bool _canGetLocation = false;

        Android.Locations.Location _location = null; // location
        double _latitude; // latitude
        double _longitude; // longitude

        // The minimum distance to change Updates in meters
        private const long MIN_DISTANCE_CHANGE_FOR_UPDATES = 10; // 10 meters

        // The minimum time between updates in milliseconds
        private const long MIN_TIME_BW_UPDATES = 1000 * 60 * 1; // 1 minute

        // Declaring a Location Manager
        protected LocationManager _locationManager;

        public GPSTracker(Context context)
        {
            _context = context;
            InitLocation();
        }

        private void InitLocation()
        {
            try
            {
                _locationManager = (LocationManager)_context.GetSystemService(LocationService);

                // getting GPS status
                _isGPSEnabled = _locationManager.IsProviderEnabled(LocationManager.GpsProvider);

                // getting network status
                _isNetworkEnabled = _locationManager.IsProviderEnabled(LocationManager.NetworkProvider);

                if (!_isGPSEnabled && !_isNetworkEnabled)
                {
                    // no network provider is enabled
                }
                else
                {
                    this._canGetLocation = true;
                    if (_isNetworkEnabled)
                    {
                        _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider,
                            MIN_TIME_BW_UPDATES,
                            MIN_DISTANCE_CHANGE_FOR_UPDATES,
                            this);

                        Log.Debug("Network", "Network Enabled");

                        if (_locationManager != null)
                        {
                            _location = _locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);

                            if (_location != null)
                            {
                                _latitude = _location.Latitude;
                                _longitude = _location.Longitude;
                            }
                        }
                    }

                    // if GPS Enabled get lat/long using GPS Services
                    if (_isGPSEnabled)
                    {
                        if (_location == null)
                        {
                            _locationManager.RequestLocationUpdates(
                                    LocationManager.GpsProvider,
                                    MIN_TIME_BW_UPDATES,
                                    MIN_DISTANCE_CHANGE_FOR_UPDATES, this);
                            Log.Debug("GPS", "GPS Enabled");

                            if (_locationManager != null)
                            {
                                _location = _locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
                                if (_location != null)
                                {
                                    _latitude = _location.Latitude;
                                    _longitude = _location.Longitude;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error("Location Error", e.StackTrace);
            }
        }

        public double Latitude
        {
            get
            {
                if (_location != null)
                {
                    _latitude = _location.Latitude;
                }

                //return _latitude;
                return 40.6878666;
            }
        }

        public double Longitude
        {
            get
            {
                if (_location != null)
                {
                    _longitude = _location.Longitude;
                }

                //return _longitude;
                return -73.9918935;
            }
        }

        /// <summary>
        /// Stop using GPS listener Calling this function will stop using GPS in your app
        /// </summary>
        public void StopUsingGps()
        {
            if (_locationManager != null)
            {
                _locationManager.RemoveUpdates(this);
            }
        }

        public bool CanGetLocation
        {
            get { return _canGetLocation; }
        }

        public void OnLocationChanged(Location location)
        { }

        public void OnProviderDisabled(string provider)
        { }

        public void OnProviderEnabled(string provider)
        { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        { }

        public override IBinder OnBind(Android.Content.Intent intent)
        {
            return null;
        }
    }
}