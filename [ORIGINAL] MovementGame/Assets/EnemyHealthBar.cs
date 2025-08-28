using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider hpBar, rallyBar;
    public Canvas canvas;
    public float hideDelay = 3;

    private void Start()
    {
        Hide();
    }
    public void Show() { 
        CancelInvoke();
        Invoke("Hide", hideDelay);

        canvas.enabled = true;
    }
    public void Hide() {
        canvas.enabled = false;
    }
    public void UpdateHp(float value) { 
        hpBar.value = value;
        Show();
    }
    public void UpdateRally(float value) { 
        rallyBar.value = value;
        Show();
    }
}
