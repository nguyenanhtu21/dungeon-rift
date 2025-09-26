using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    public Button buttonAttack;
    public Button buttonHP;
    public Button buttonMP;

    public float healAmount = 30f;
    public float manaAmount = 30f;

    private Character character;

    void Start()
    {
        character = FindAnyObjectByType<Character>();

        if (character != null)
        {
            if (buttonAttack != null)
                buttonAttack.onClick.AddListener(OnAttackClick);

            if (buttonHP != null)
                buttonHP.onClick.AddListener(OnHealClick);

            if (buttonMP != null)
                buttonMP.onClick.AddListener(OnManaClick);
        }
        else
        {
            return;
        }
    }

    private void OnAttackClick()
    {
        if (character != null)
        {
            character.Attack();
        }
    }

    private void OnHealClick()
    {
        if (character != null)
        {
            character.Heal(healAmount);
        }
    }

    private void OnManaClick()
    {
        if (character != null)
        {
            character.RestoreMana(manaAmount);
        }
    }
}

