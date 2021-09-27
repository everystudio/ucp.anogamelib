using UnityEngine;

namespace anogamelib
{
    public interface IMove2D
    {
        void OnMove(Vector2 direction, float velocity);
    }
}
