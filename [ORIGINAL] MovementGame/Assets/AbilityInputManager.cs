using UnityEngine;
using CustomClasses;
using System.Numerics;
using UnityEngine.UIElements;


public class AbilityInputManager : MonoBehaviour
{
    public bool jumpD, lightD, heavyD, altD;
    public bool jump, light_, heavy, alt;
    public KeyCode jumpKey, lightKey, heavyKey, altKey, nextKey, prevKey, castKey, QAKey1, QAKey2, QAKey3, attackKey;

    public Object[] _abilities;
    public IAbility[] abilities;
    public Object _attack, _QAbility1, _QAbility2, _QAbility3;
    public IAbility attack, QAbility1, QAbility2, QAbility3;

    public int abilityNum;
    public IAbility currentAbility;
    public CastInfo queuedAbility;
    public bool queued, r = false;
    public float queueWindow = 1f;
    public BetterController player;
    public bool canScroll;
    public float scrollCd = 0.1f;

    public void resetScroll()
    {
        canScroll = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
        player = GetComponent<BetterController>();
        //abilities.Length = tmp.Length;
        abilities = new IAbility[_abilities.Length];
        for (int i = 0; i < _abilities.Length; i++) {
            abilities[i] = (IAbility) _abilities[i];
        }
        QAbility1 = (IAbility)_QAbility1;
        QAbility2 = (IAbility)_QAbility2;
        QAbility3 = (IAbility)_QAbility3;
        attack = (IAbility)_attack;
        //Debug.Log(abilities.Length);
        currentAbility = abilities[abilityNum];
    }

    void DoCast(IAbility toCast, KeyCode key) {
        toCast.SetKey(key);

        if (light_ && false)
        {
            toCast.LightCast();
        }
        else if (heavy)
        {
            toCast.HeavyCast();
        }
        else toCast.Cast();
    }

    // Update is called once per frame
    void Update()
    {
        

        jumpD = Input.GetKeyDown(jumpKey);
        //lightD = Input.GetKeyDown(lightKey);
        heavyD = Input.GetKeyDown(heavyKey);
        //altD = Input.GetKeyDown(altKey);

        jump = Input.GetKey(jumpKey);
        //light_ = Input.GetKey(lightKey);
        heavy = Input.GetKey(heavyKey);
        //alt = Input.GetKey(altKey);

        if (Input.GetKeyDown(nextKey) && canScroll)
        {
            canScroll = false;
            Invoke("resetScroll", scrollCd);

            abilityNum = (abilityNum + 1) % abilities.Length;

            currentAbility = abilities[abilityNum];
        }
        if (Input.GetKeyDown(prevKey) && canScroll)
        {
            canScroll = false;
            Invoke("resetScroll", scrollCd);

            if (abilityNum == 0) abilityNum = abilities.Length - 1;
            else abilityNum = abilityNum - 1;

            currentAbility = abilities[abilityNum];
        }

        if (Input.GetKeyDown(castKey) & currentAbility.IsReady()) {
            DoCast(currentAbility, castKey);
        }
        if (Input.GetKeyDown(QAKey1))
        {
            if (QAbility1.IsReady())DoCast(QAbility1, QAKey1);
        }
        if (Input.GetKeyDown(QAKey2))
        {
            if(QAbility2.IsReady()) DoCast(QAbility2, QAKey2);
        }
        if (Input.GetKeyDown(QAKey3))
        {
            if(QAbility3.IsReady()) DoCast(QAbility3, QAKey3);
        }
        if (Input.GetKeyDown(attackKey))
        {
            if(attack.IsReady()) DoCast(attack, attackKey);
        }
    }

    
}
