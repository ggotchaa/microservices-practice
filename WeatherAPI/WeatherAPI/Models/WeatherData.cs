using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class WeatherData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Temperature { get; set; }

    public string Description { get; set; }

    public DateTime Timestamp { get; set; }
}
