using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    //Info
    public string characterName;
    public Sprite portrait;
    public GameObject prefab;

    //Stats
    public int maxHP = 100;
    public int maxMana = 50;
    public int normalAttackDamage = 10;
    public int skillDamage = 30;
    public int skillManaCost = 20;
}
