using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHPLogic : HPLogic
{
    [SerializeField]
    [Range(0, 1)]
    private float _shipPossessChance = 0.33f;

    [SerializeField]
    private GameObject _spaceShipPrefab;

    private bool _hasDied = false;

    protected override void ExplodeOnDeath()
    {
        if (_hasDied) return;

        //Debug.Log($"{gameObject.name} died");
        WaveManager.Instance.DecreaseEnemyCount();

        if (WilTurnIntoSpaceship(_shipPossessChance) && _spaceShipPrefab != null)
        {
            Die();
        }
        else
            base.ExplodeOnDeath();

        _hasDied = true;
    }

    private void Die()
    {
        Instantiate(_spaceShipPrefab, transform.position, transform.rotation);
        Destroy(gameObject); //eventually make a pool pattern please
    }

    private bool WilTurnIntoSpaceship(float shipPossessChance)
    {
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Possession");

        if (ships.Count() > GameManager.maxShips)
            return false;

        float chance = Random.Range(0f, 1f);

        return chance <= _shipPossessChance;
    }
}
