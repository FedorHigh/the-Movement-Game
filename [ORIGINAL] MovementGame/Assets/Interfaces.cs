using UnityEngine;

namespace Interfaces
{
    public interface IAbility
    {
        int GetID();
        void LightCast(KeyCode key);
        void Cast(KeyCode key);
        void HeavyCast(KeyCode key);
        void Reset();

    }
}
