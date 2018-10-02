using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace SpotifyRecommend.SpotifyService.ResponseObjects
{
    public static class SpotifyResponseObjectConverter
    {
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Converter.Settings);
        }

        public static string ToJson<T>(T objectToConvert)
        {
            return JsonConvert.SerializeObject(objectToConvert, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                DateConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class DateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ReleaseDate) || t == typeof(ReleaseDate?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    DateTimeOffset dt;
                    if (DateTimeOffset.TryParse(stringValue, out dt))
                    {
                        return new ReleaseDate { DateTime = dt };
                    }
                    long l;
                    if (Int64.TryParse(stringValue, out l))
                    {
                        return new ReleaseDate { Integer = l };
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type Date");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (ReleaseDate)untypedValue;
            if (value.DateTime != null)
            {
                serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
                return;
            }
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value.ToString());
                return;
            }
            throw new Exception("Cannot marshal type ReleaseDate");
        }

        public static readonly DateConverter Singleton = new DateConverter();
    }
}