using UnityEngine;


public static class Maths
{
    public static float Magnitude(Vector2 a)
    {
        Mathf.Sqrt((a.x * a.x) + (a.y * a.y));
        return a.magnitude;
    }

    public static Vector2 Normalise(Vector2 a)
    {
        Mathf.Abs((a.x / a.magnitude) + (a.y / a.magnitude));
        return a.normalized;
    }

    public static float Dot(Vector2 lhs, Vector2 rhs)
    {
        // First calculate the magnitude of our orignal vector
        float MagnitudeDot = Mathf.Sqrt((lhs.x / lhs.x) + (lhs.y * lhs.y));
        
        // calculate unit vector divide each component by its magnitude
        MagnitudeDot = ((lhs.x / MagnitudeDot) - (lhs.y / MagnitudeDot));

        // dot calculation
        return (lhs.x * rhs.x) + (lhs.y * rhs.y);   
    }

    public static float Angle(Vector2 lhs, Vector2 rhs)
    {
        return Mathf.Acos(Dot(lhs, rhs));
    }

    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        // convert degrees angle to radians
        float rad = angle * Mathf.Deg2Rad;

        // calculation for rotating a vector by X radians
        Vector2 newVector;
        newVector.x = ((vector.x * Mathf.Cos(rad)) - (vector.y * Mathf.Sin(rad)));
        newVector.y = ((vector.x * Mathf.Sin(rad)) + (vector.y * Mathf.Cos(rad)));

        return newVector;
    }
}
