using UnityEngine;

public class FillText : MonoBehaviour
{
    public TextMesh curAbilityText;
    public AbilityInputManager abilityInputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curAbilityText.text = abilityInputManager.abilities[abilityInputManager.currentAbility].GetID().ToString();
    }
}
