﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects
{
    public class Geolocalizacao
    {
        [JsonPropertyName("latidude")]
        public decimal Latidude { get; set; }

        [JsonPropertyName("longitude")]
        public decimal Longitude { get; set; }
    }
}
