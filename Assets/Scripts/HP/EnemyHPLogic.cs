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
    public int ShipQuality = 1;

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

        //GameManager.PossessableShips.Add(spaceShip);

        //SetLowestQuality();


        GameManager.PossessableShipsDictionary.Add(spaceShip, ShipQuality);

        //Debug.Log($"Total Qualities: {GameManager.PossessableShipsDictionary.Count}, Highest Quality: {GameManager.ReturnHighestQuality()}, Lowest Quality: {GameManager.ReturnLowestQuality()}");


        Destroy(gameObject); //eventually make this a pool pattern please
    }

    //private void SetLowestQuality()
    //{
    //    if (GameManager.PossessableShips.Count() > GameManager.MaxPossessableShips)
    //    {
    //        if (GameManager.ShipQualites.Min() < ShipQuality)
    //        {
    //            GameManager.ShipQualites.Remove(GameManager.ShipQualites.Min());
    //            GameManager.ShipQualites.Add(ShipQuality);
    //            //this doesnt work as the quality doesnt get removed when a possessable ship explodes
    //        }
    //    }
    //    else
    //        GameManager.ShipQualites.Add(ShipQuality);
    //}

    private bool WilTurnIntoSpaceship(float shipPossessChance)
    {
        if (!IsBetterQuality())
        {
            if (GameManager.PossessableShipsDictionary.Count() > GameManager.MaxPossessableShips)
            {
                Debug.Log("Can't spawn spaceship");
                return false;
            }
        }

        if (GameManager.DebugMode)
            return true;

        float possessChanceOffset = -0.2f + ((GameManager.WaveCount + (float)Mathf.Pow(1.06f, GameManager.WaveCount)) / 70);

        //lets make this more sophisticated
        float chance = Random.Range(0f, 1f);

        //Debug.Log($"{chance}, {possessChanceOffset}, {chance + possessChanceOffset}");

        return chance + possessChanceOffset <= _shipPossessChance;
    }

    private bool IsBetterQuality()
    {
        if (GameManager.ReturnLowestQuality() < ShipQuality)
            return true;

        return false;
    }
}
