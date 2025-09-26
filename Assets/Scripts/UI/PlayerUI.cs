using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image healthFill;
    public Image manaFill;

    public void SetHealth(float current, float max)
    {
        healthFill.fillAmount = current / max;
    }

    public void SetMana(float current, float max)
    {
        manaFill.fillAmount = current / max;
    }
}
