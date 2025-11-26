using System.Collections;
using UnityEngine;

public class AutomaticShoot : MonoBehaviour, IShootable
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private float _damage = 10f;

    private bool _previousAttack;

    private float _shootTimer;
    public float _shootDelay = 0.1f;
    private bool _canAttack = true;

    public void OnRequestAttack(bool attack)
    {
        if (attack && _canAttack)
            StartCoroutine(AttackWithDelay(_shootDelay));
    }

    public IEnumerator AttackWithDelay(float delay)
    {
        _canAttack = false;
        Attack();
        yield return new WaitForSeconds(delay);
        _canAttack = true;
    }

    public void Attack()
    {
        BulletController bulletController = _bullet.GetComponent<BulletController>();

        bulletController.BulletDamage = _damage;

        Instantiate(_bullet, transform.position, transform.rotation);
    }
}
