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
    [Range(0, 6)]
    public int PossessQuality = 1;

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
            TurnIntoPossessable();
        }
        else
            base.ExplodeOnDeath();

        _hasDied = true;
    }

    private void TurnIntoPossessable()
    {
        GameObject spaceShip = Instantiate(_spaceShipPrefab, transform.position, transform.rotation);

        GameManager.PossessableSpaceShips.Add(spaceShip);

        if (GameManager.LowestShipQuality > PossessQuality)
            GameManager.LowestShipQuality = PossessQuality;

        Debug.Log($"Lowest Quality: {GameManager.LowestShipQuality}");

        Destroy(gameObject); //eventually make this a pool pattern please
    }

    private bool WilTurnIntoSpaceship(float shipPossessChance)
    {
        if (!IsBetterQuality())
        {
            if (GameManager.PossessableSpaceShips.Count() > GameManager.MaxPossessableShips)
            {
                Debug.Log("Can't spawn spaceship");
                return false;
            }
        }

        float chance = Random.Range(0f, 1f);

        return chance <= _shipPossessChance;
    }

    private bool IsBetterQuality()
    {
        if (GameManager.LowestShipQuality < PossessQuality)
            return true;

        return false;
    }
}
