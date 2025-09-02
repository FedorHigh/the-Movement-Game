using UnityEngine;
using UnityEngine.UI;
using CustomClasses;
using TMPro;

public class UICooldown : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private GameObject noManaOverlay, noStaminaOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText; 
    //public Color dull = Color.gray;
    private float cooldownDuration = 0f;
    private float currentCooldown = 0f;
    private bool isOnCooldown = false;
    public BetterController player;
    public Ability ability;
    public bool debug = false;

    void Start()
    {
        if(player==null)player = GlobalVars.instance.player;
    }

    void Update()
    {
        if (debug) {
            Debug.Log("UI debug cur stamina: " + player.hpManager.stamina);
            Debug.Log("UI debugneeded stamina: " + ability.defaultStamina);
        }
        noManaOverlay.SetActive(player.hpManager.mana < ability.defaultMana);
        noStaminaOverlay.SetActive(!noManaOverlay.activeSelf && player.hpManager.stamina < ability.defaultStamina); // OPTIMISE 
        

        if (ability != null)
        {
            cooldownDuration = ability.CDset;
            currentCooldown = ability.CDleft;

            if (currentCooldown > 0)
            {
                StartCooldown();
            }
        }

        if (isOnCooldown)
        {

            if (currentCooldown <= 0f)
            {
                EndCooldown();
            }
            else
            {
                float rounded = Mathf.Ceil(currentCooldown);
                cooldownText.text = rounded.ToString();
                cooldownOverlay.fillAmount = currentCooldown / cooldownDuration;
            }
        }
    }

    public void StartCooldown()
    {
        isOnCooldown = true;
        cooldownOverlay.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(true);
    }

    private void EndCooldown()
    {
        isOnCooldown = false;
        cooldownOverlay.fillAmount = 0f;
        cooldownText.text = "";

        cooldownOverlay.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    // Метод для обновления текущего кулдауна вручную
    public void UpdateFromAbility(Ability newAbility)
    {
        ability = newAbility;
        cooldownDuration = ability.CDset;
        currentCooldown = ability.CDleft;

        if (currentCooldown > 0)
        {
            StartCooldown();
        }
        else
        {
            EndCooldown();
        }
    }
}