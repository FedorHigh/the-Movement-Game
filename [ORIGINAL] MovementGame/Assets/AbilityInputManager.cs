using UnityEngine;
using Interfaces;
using System.Numerics;


public class AbilityInputManager : MonoBehaviour
{
    public bool jumpD, lightD, heavyD, altD;
    public bool jump, light_, heavy, alt;
    public KeyCode jumpKey, lightKey, heavyKey, altKey, nextKey, prevKey, castKey;
    public Object[] tmp;
    public IAbility[] abilities;
    public int abilityNum;
    public IAbility currentAbility;
    public CastInfo queuedAbility;
    public bool queued, r = false;
    public float queueWindow = 1f;
    public BetterController player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<BetterController>();
        //abilities.Length = tmp.Length;
        abilities = new IAbility[tmp.Length];
        for (int i = 0; i < tmp.Length; i++) {
            abilities[i] = (IAbility) tmp[i];
        }
        //Debug.Log(abilities.Length);
        currentAbility = abilities[abilityNum];
    }
    

    // Update is called once per frame
    void Update()
    {
        

        jumpD = Input.GetKeyDown(jumpKey);
        lightD = Input.GetKeyDown(lightKey);
        heavyD = Input.GetKeyDown(heavyKey);
        altD = Input.GetKeyDown(altKey);

        jump = Input.GetKey(jumpKey);
        light_ = Input.GetKey(lightKey);
        heavy = Input.GetKey(heavyKey);
        alt = Input.GetKey(altKey);

        if (Input.GetKeyDown(nextKey))
        {
            abilityNum = (abilityNum + 1) % abilities.Length;

            currentAbility = abilities[abilityNum];
        }
        if (Input.GetKeyDown(prevKey))
        {
            if(abilityNum == 0) abilityNum = abilities.Length - 1;
            else abilityNum = abilityNum - 1;

            currentAbility = abilities[abilityNum];
        }

        if (Input.GetKeyDown(castKey) & currentAbility.IsReady()) {
            currentAbility.SetKey(castKey);

            if (light_) {
                currentAbility.LightCast();
            }
            else if (heavy)
            {
                currentAbility.HeavyCast();
            }
            else currentAbility.Cast();
        }
    }

    
}
