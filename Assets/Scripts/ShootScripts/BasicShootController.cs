using UnityEngine;

public class BasicShootController : MonoBehaviour, IShootable
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private float _damage = 10f;

    private bool _currentAttack;
    private bool _previousAttack;

    public void OnRequestAttack(bool attack)
    {
        if(attack && _previousAttack == false)
            Attack();

        _previousAttack = attack;
    }

    public void Attack()
    {
        BulletController bulletController = _bullet.GetComponent<BulletController>();

        bulletController.BulletDamage = _damage;

        Instantiate(_bullet, transform.position, transform.rotation);
    }
}
