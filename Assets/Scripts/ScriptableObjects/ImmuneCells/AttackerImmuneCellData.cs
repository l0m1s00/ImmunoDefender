using UnityEngine;

public class AttackerImmuneCellData : ImmuneCellData
{
    [Header("Combat Stats")]
    public float damage;
    public float attackRange; //distancia a la que se detiene y ataca
    public float attackCooldown; //tiempo de espera entre golpe y golpe
}
