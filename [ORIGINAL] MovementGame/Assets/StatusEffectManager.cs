using CustomClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    public Dictionary<String, StatusEffect> activeEffects = new Dictionary<String, StatusEffect>();
    public BetterController player;
    public Entity entity;
    public void Start()
    {
        TryGetComponent<BetterController>(out player);
        TryGetComponent<Entity>(out entity);
    }
    public void AddEffect(StatusEffect effect) {

        Debug.Log("adding effect " + effect.effectName);
        if (activeEffects.ContainsKey(effect.effectName))//sole reason to use dict, mb change later?
        {
            activeEffects[effect.effectName].AddStack();
        }
        else
        {
            GameObject instance = Instantiate(effect.gameObject, transform);
            effect = instance.GetComponent<StatusEffect>();
            effect.manager = this;
            activeEffects[effect.effectName] = effect;
        }
    }

    public void RemoveEffect(StatusEffect effect) {
        Debug.Log("removing effect " + effect.effectName);
        if (activeEffects.ContainsKey(effect.effectName))//sole reason to use dict, mb change later?
        {
            Destroy(activeEffects[effect.effectName].gameObject);
            activeEffects.Remove(effect.effectName);
        }
    }
}
