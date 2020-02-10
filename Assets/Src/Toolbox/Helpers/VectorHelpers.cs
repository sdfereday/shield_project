using UnityEngine;

namespace Game.Toolbox.Helpers
{
    public static class Vectors
    {
        public static float Dist(Vector2 a, Vector2 b) => Vector2.Distance(a, b);
        public static float Dist(Vector3 a, Vector3 b) => Vector3.Distance(a, b);
    }
}