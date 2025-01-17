﻿
using SplashKitSDK;

namespace Cave_dweller
{
    public static class VectorUtils
    {
        // Subtracts two vectors and returns the result
        public static Vector2D SubtractVectors(Vector2D v1, Vector2D v2)
        {
            return new Vector2D() { X = v1.X - v2.X, Y = v1.Y - v2.Y };
        }

        // Calculates the distance between two vectors
        public static double DistanceTo(Vector2D from, Vector2D to)
        {
            double dx = from.X - to.X;
            double dy = from.Y - to.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
