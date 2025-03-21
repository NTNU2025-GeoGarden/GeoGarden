using System;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using System.IO;
using UnityEngine;
using Random = System.Random;

public class CoordinateGenerator
{
    private static Random random = new Random(DateTime.Now.Year * 1000 + DateTime.Now.DayOfYear);
    private const double EarthRadius = 6378137.0; // Earth's radius in meters

    // Method to get the bounding box based on center coordinates and distance
    private static Tuple<Tuple<double, double>, Tuple<double, double>> GetBoundingBox(double centerLat, double centerLng, double distanceMeters)
    {
        double latOffset = (distanceMeters / EarthRadius) * (180.0 / Math.PI);
        double lngOffset = (distanceMeters / EarthRadius) * (180.0 / Math.PI) / Math.Cos(centerLat * Math.PI / 180.0);

        double topLeftLat = centerLat + latOffset;
        double topLeftLng = centerLng - lngOffset;
        Tuple<double, double> topLeft = Tuple.Create(topLeftLat, topLeftLng);

        double bottomRightLat = centerLat - latOffset;
        double bottomRightLng = centerLng + lngOffset;
        Tuple<double, double> bottomRight = Tuple.Create(bottomRightLat, bottomRightLng);

        return Tuple.Create(topLeft, bottomRight);
    }

    // Public method to generate unique points and write them to a file
    public static List<LatitudeLongitude> GenerateUniquePoints(double centerLat, double centerLng, double distanceMeters, int numPoints = 5, double minDistance = 0.001)
    {
        var boundingBox = GetBoundingBox(centerLat, centerLng, distanceMeters);
        var points = new List<LatitudeLongitude>(numPoints);
        var maxAttempts = numPoints * 5; // Limit attempts to prevent infinite loops
        var attempts = 0;

        while (points.Count < numPoints && attempts < maxAttempts)
        {
            attempts++;
            double lat = random.NextDouble() * (boundingBox.Item2.Item1 - boundingBox.Item1.Item1) + boundingBox.Item1.Item1;
            double lon = random.NextDouble() * (boundingBox.Item2.Item2 - boundingBox.Item1.Item2) + boundingBox.Item1.Item2;
            var newPoint = new LatitudeLongitude(lat, lon);

            // Quick check if point is too close to existing points
            bool isFarEnough = true;
            foreach (var point in points)
            {
                if (CalculateDistance(newPoint.Latitude, newPoint.Longitude, point.Latitude, point.Longitude) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }

            if (isFarEnough) points.Add(newPoint);
        }

        Debug.Log($"<color=cyan>[CoordinateGenerator] Generated {points.Count} points in {attempts} attempts</color>");
        return points;
    }

    // Private method to calculate distance using the Haversine formula
    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Radius of the Earth in kilometers
        var latRadians1 = DegreesToRadians(lat1);
        var latRadians2 = DegreesToRadians(lat2);
        var latRadiansDelta = DegreesToRadians(lat2 - lat1);
        var lonRadiansDelta = DegreesToRadians(lon2 - lon1);

        var a = Math.Sin(latRadiansDelta / 2) * Math.Sin(latRadiansDelta / 2) +
                Math.Cos(latRadians1) * Math.Cos(latRadians2) *
                Math.Sin(lonRadiansDelta / 2) * Math.Sin(lonRadiansDelta / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    // Helper method to convert degrees to radians
    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}
