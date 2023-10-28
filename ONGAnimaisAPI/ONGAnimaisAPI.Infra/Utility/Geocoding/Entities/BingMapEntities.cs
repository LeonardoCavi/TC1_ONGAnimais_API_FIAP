using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Infra.Utility.Geocoding.Entities
{
    public class BingMapsResponse
    {
        [JsonPropertyName("resourceSets")]
        public ResourceSets[] ResourceSets { get; set; }
    }

    public class ResourceSets
    {
        [JsonPropertyName("resources")]
        public Resources[] Resources { get; set; }
    }

    public class Resources
    {
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("geocodePoints")]
        public GeocodePoints[] GeocodePoints { get; set; }
    }

    public class GeocodePoints
    {
        [JsonPropertyName("coordinates")]
        public decimal[] Coordinates { get; set; }
    }


    public class Address
    {
        public string addressLine { get; set; }
        public string adminDistrict { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public Intersection intersection { get; set; }
        public string locality { get; set; }
        public string postalCode { get; set; }
    }

    public class Intersection
    {
        public string baseStreet { get; set; }
        public string secondaryStreet1 { get; set; }
        public string intersectionType { get; set; }
        public string displayName { get; set; }
    }
}
