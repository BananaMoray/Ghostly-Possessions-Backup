using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHPLogic : HPLogic
{
    [SerializeField]
    [Range(0, 1)]
    private float _shipPossessChance = 0.33f;

    [SerializeField]
    private GameObject _spaceShipPrefab;

    protected override void ExplodeOnDeath()
    {
        if (TurnIntoSpaceship(_shipPossessChance) && _spaceShipPrefab != null)
        {
            Instantiate(_spaceShipPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
            base.ExplodeOnDeath();
    }

    private bool TurnIntoSpaceship(float shipPossessChance)
    {
        float chance = Random.Range(0f, 1f);

        return chance <= _shipPossessChance;
    }
}
