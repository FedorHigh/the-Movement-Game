using CustomClasses;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Script : MonoBehaviour
{
    public ProgressBar cooldownBar;
    public AbilityInputManager manager;
    public IAbility ability;
    public float curValue;
    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        cooldownBar = doc.rootVisualElement.Q<ProgressBar>("cooldownBar");
    }

    private void Update()
    {
        //ability = inputManager.currentAbility;
        //Debug.Log(ability.GetCDleft());
        //Debug.Log(ability.GetCDset());
        //curValue = ability.GetCDleft() / ability.GetCDset();
        //cooldownBar.value = curValue;
    }
}
