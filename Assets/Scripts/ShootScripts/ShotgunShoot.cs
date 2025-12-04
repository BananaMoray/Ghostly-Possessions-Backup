using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : AutomaticShoot
{
    [SerializeField] private int pelletCount = 3;
    [SerializeField] private float spreadAngle = 30f;

    public override void Attack()
    {
        //we call the basic attack so we can override the alt attack
        Attack(pelletCount, spreadAngle);
    }

    public override void Attack(int amount, float angle)
    {
        base.Attack(amount, angle);
    }
}
