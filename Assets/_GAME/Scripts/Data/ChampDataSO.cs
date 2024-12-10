using UnityEngine;

public class ChampDataSO : ScriptableObject{
    [Header("Default")]
    [SerializeField] protected string name;
    [SerializeField] protected int health;
    [SerializeField] protected int attack;
    [SerializeField] protected float aSpeed;
    [SerializeField] protected float aRange;
    [SerializeField] protected float detectionRadius;
    
    [Header("Bonus")]
    [SerializeField] protected int hpBonus;
    [SerializeField] protected int attackBonus;

    public int HpBonus => hpBonus;
    public int AttackBonus => attackBonus;
    
    public string Name => name;
    public int Health => health;
    public int Attack => attack;
    public float ASpeed => aSpeed;
    public float ARange => aRange;
    public float DetectionRadius => detectionRadius;
}