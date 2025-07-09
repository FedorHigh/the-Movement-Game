using UnityEngine;
using UnityEngine.UI;
using CustomClasses;
using TMPro;

public class UICooldown : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay; // Filled Image
    [SerializeField] private TextMeshProUGUI cooldownText;     // ������� UI Text
    private float cooldownDuration = 0f;
    private float currentCooldown = 0f;
    private bool isOnCooldown = false;

    public Ability ability;

    void Start()
    {
        
    }

    void Update()
    {
        // �������� �������� �� �������� ������
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

    // ����� ��� ���������� �������� �������� �������
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