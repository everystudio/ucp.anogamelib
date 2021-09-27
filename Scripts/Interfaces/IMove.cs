using UnityEngine;

namespace anogamelib
{
    public interface IMove2D
    {
        void OnMoveHandle(Vector2 direction, float velocity);
    }
}
