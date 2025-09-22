using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fill;

    public void SetHealth(float current, float max)
    {
        float ratio = Mathf.Clamp01(current / max);
        fill.fillAmount = ratio;
    }
}
