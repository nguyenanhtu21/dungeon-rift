[System.Serializable]
public class CharacterStats
{
    public int Strength;
    public int Agility;
    public int Intelligence;
    public int Endurance;
    public int Luck;

    public float AttackPower => Strength * 2;
    public float Defense => Endurance * 1.5f;
    public float CritRate => Luck * 0.05f;
}
