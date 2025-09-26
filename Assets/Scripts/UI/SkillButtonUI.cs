using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButtonUI : MonoBehaviour
{
    [Header("UI References")]
    public Image cooldownOverlay;
    public TextMeshProUGUI cooldownText;
    public Button button;
    public Image iconImage;

    private Character character;
    private float cooldownTime;
    private float cooldownRemaining;

    void Start()
    {
        character = FindAnyObjectByType<Character>();

        if (character != null && character.data != null)
        {
            if (character.data.skillIcon != null)
            {
                iconImage.sprite = character.data.skillIcon;
            }
        }

        cooldownOverlay.fillAmount = 0;
        cooldownText.text = "";
    }

    void Update()
    {
        if (character == null || character.data == null) return;

        // Cooldown logic
        if (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            float ratio = cooldownRemaining / cooldownTime;
            cooldownOverlay.fillAmount = ratio;
            cooldownText.text = Mathf.CeilToInt(cooldownRemaining).ToString();

            if (cooldownRemaining <= 0)
            {
                cooldownOverlay.fillAmount = 0;
                cooldownText.text = "";
                button.interactable = true;
            }
        }

        // Mana check
        if (character.CurrentMana < character.data.skillManaCost)
        {
            button.interactable = false;
            iconImage.color = Color.gray;
        }
        else if (cooldownRemaining <= 0)
        {
            button.interactable = true;
            iconImage.color = Color.white;
        }
    }

    public void OnClickSkill()
    {
        if (character == null) return;

        bool used = character.UseSkill();
        if (used)
        {
            cooldownTime = character.data.skillCooldown;
            cooldownRemaining = cooldownTime;

            cooldownOverlay.fillAmount = 1;
            button.interactable = false;
        }
    }
}
