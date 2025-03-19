using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

public class CoordinateGenerator
{
    private static Random random = new Random();
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
    public static void GenerateUniquePointsAndWriteToFile(double centerLat, double centerLng, double distanceMeters, string filePath, int numPoints = 5, double minDistance = 0.001)
    {
        try 
        {
            var boundingBox = GetBoundingBox(centerLat, centerLng, distanceMeters);
            var points = new List<Tuple<double, double>>(numPoints);
            var maxAttempts = numPoints * 10; // Limit attempts to prevent infinite loops
            var attempts = 0;

            while (points.Count < numPoints && attempts < maxAttempts)
            {
                attempts++;
                double lat = random.NextDouble() * (boundingBox.Item2.Item1 - boundingBox.Item1.Item1) + boundingBox.Item1.Item1;
                double lon = random.NextDouble() * (boundingBox.Item2.Item2 - boundingBox.Item1.Item2) + boundingBox.Item1.Item2;
                var newPoint = Tuple.Create(Math.Round(lat, 6), Math.Round(lon, 6));

                // Quick check if point is too close to existing points
                bool isFarEnough = true;
                foreach (var point in points)
                {
                    if (CalculateDistance(newPoint.Item1, newPoint.Item2, point.Item1, point.Item2) < minDistance)
                    {
                        isFarEnough = false;
                        break;
                    }
                }

                if (isFarEnough)
                {
                    points.Add(newPoint);
                }
            }

            // Write points directly without creating intermediate strings
            using (var writer = new StreamWriter(filePath, false))
            {
                foreach (var point in points)
                {
                    writer.WriteLine($"{point.Item1}, {point.Item2}");
                }
            }

            Debug.Log($"<color=cyan>[CoordinateGenerator] Generated {points.Count} points in {attempts} attempts</color>");
        }
        catch (Exception e)
        {
            Debug.LogError($"[CoordinateGenerator] Error generating coordinates: {e.Message}");
        }
    }

    // Helper method to generate unique points
    private static List<Tuple<double, double>> GenerateUniquePoints(double centerLat, double centerLng, double distanceMeters, int numPoints, double minDistance)
    {
        var boundingBox = GetBoundingBox(centerLat, centerLng, distanceMeters);
        var topLeft = boundingBox.Item1;
        var bottomRight = boundingBox.Item2;

        var points = new List<Tuple<double, double>>();

        while (points.Count < numPoints)
        {
            double lat = Math.Round(random.NextDouble() * (bottomRight.Item1 - topLeft.Item1) + topLeft.Item1, 6);
            double lon = Math.Round(random.NextDouble() * (bottomRight.Item2 - topLeft.Item2) + topLeft.Item2, 6);
            var newPoint = Tuple.Create(lat, lon);

            bool isFarEnough = true;
            foreach (var existingPoint in points)
            {
                if (CalculateDistance(newPoint.Item1, newPoint.Item2, existingPoint.Item1, existingPoint.Item2) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }

            if (isFarEnough)
            {
                points.Add(newPoint);
            }
        }

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
