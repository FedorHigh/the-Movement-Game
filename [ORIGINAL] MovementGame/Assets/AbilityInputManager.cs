using UnityEngine;
using Interfaces;
using System.Numerics;


public class AbilityInputManager : MonoBehaviour
{
    public bool jumpD, lightD, heavyD, altD;
    public bool jump, light, heavy, alt;
    public KeyCode jumpKey, lightKey, heavyKey, altKey, nextKey, prevKey, castKey;
    public Object[] tmp;
    public IAbility[] abilities;
    public int currentAbility;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //abilities.Length = tmp.Length;
        abilities = new IAbility[tmp.Length];
        for (int i = 0; i < tmp.Length; i++) {
            abilities[i] = (IAbility) tmp[i];
        }
        //Debug.Log(abilities.Length);
    }

    // Update is called once per frame
    void Update()
    {
        jumpD = Input.GetKeyDown(jumpKey);
        lightD = Input.GetKeyDown(lightKey);
        heavyD = Input.GetKeyDown(heavyKey);
        altD = Input.GetKeyDown(altKey);

        jump = Input.GetKey(jumpKey);
        light = Input.GetKey(lightKey);
        heavy = Input.GetKey(heavyKey);
        alt = Input.GetKey(altKey);

        if (Input.GetKeyDown(nextKey))
        {
            currentAbility = (currentAbility + 1) % abilities.Length;
        }
        if (Input.GetKeyDown(prevKey))
        {
            if(currentAbility == 0) currentAbility = abilities.Length - 1;
            else currentAbility = currentAbility-1;
        }

        if (Input.GetKeyDown(castKey)) {
            //Debug.Log(abilities[0].ToString());
            //Debug.Log(abilities[1].ToString());
            if (light) {
                abilities[currentAbility].LightCast();
            }
            else if (heavy)
            {
                abilities[currentAbility].HeavyCast();
            }
            else abilities[currentAbility].Cast();
        }
    }

    
}
