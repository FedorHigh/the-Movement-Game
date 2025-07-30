using CustomClasses;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    public static class EventManager { 
        // enemies
        public static UnityAction<HitInfo> OnEnemyHit;
    }
}