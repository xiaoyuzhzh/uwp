using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Weather
{
    class LocationService
    {

        public async Task<Geoposition> getLocation()
        {
            var access = await Geolocator.RequestAccessAsync();

            if (!access.Equals(GeolocationAccessStatus.Allowed))
            {
                throw new Exception("no right to get location");
            }


            var geolocator = new Geolocator() { DesiredAccuracyInMeters = 0};

            var position = await geolocator.GetGeopositionAsync();


            return position;
        }

    }
}
