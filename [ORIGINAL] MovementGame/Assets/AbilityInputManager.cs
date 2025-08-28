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
    [SerializeField] private Ability[] abilities; // FOR INPUT ONLY!!!
    public Object _attack, _QAbility1, _QAbility2, _QAbility3;
    public Ability attack, QAbility1, QAbility2, QAbility3;

    public int abilityNum;
    public Ability currentAbility;
    public CastInfo queuedAbility;
    public bool queued, r = false;
    public float queueWindow = 1f;
    public BetterController player;
    public bool canScroll;
    public float scrollCd = 0.1f;
    public KeyCode sprintKey = KeyCode.LeftAlt;
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
        abilities = new Ability[_abilities.Length];
        for (int i = 0; i < _abilities.Length; i++) {
            abilities[i] = (Ability) _abilities[i];
        }
        QAbility1 = (Ability)_QAbility1;
        QAbility2 = (Ability)_QAbility2;
        QAbility3 = (Ability)_QAbility3;
        attack = (Ability)_attack;
        //Debug.Log(abilities.Length);
        currentAbility = abilities[abilityNum];
    }

    void DoCast(Ability toCast, KeyCode key) {
        //if (player.dashing) return;
        toCast.SetKey(key);

        if (heavy)
        {
            //if (!player.hpManager.UseManaAndStamina(toCast.heavyMana, toCast.heavyStamina)) return;

            toCast.ProcessInput(1);
        }
        else
        {
            //if (!player.hpManager.UseManaAndStamina(toCast.defaultMana, toCast.defaultStamina)) return;

            toCast.ProcessInput(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //sprinting
        if (Input.GetKeyDown(sprintKey)) player.sprinting = !player.sprinting;

        //abilities
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
        if (Input.GetKey(QAKey1))
        {
            DoCast(QAbility1, QAKey1);
        }
        if (Input.GetKeyDown(QAKey2))
        {
            DoCast(QAbility2, QAKey2);
        }
        if (Input.GetKeyDown(QAKey3))
        {
            DoCast(QAbility3, QAKey3);
        }
        if (Input.GetKeyDown(attackKey))
        {
            DoCast(attack, attackKey);
        }
    }

    
}
