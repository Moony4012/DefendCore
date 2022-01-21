using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [Header("Entity - Hp")]
    [SerializeField]
    private int         _hp = 3;
    [SerializeField]
    private GameObject _lifeBarPrefab;
    [SerializeField]
    private GameObject _lifeBarAnchor;

    [Header("Entity - Bullet")]
    [SerializeField]
    private GameObject  _bulletPrefab = null;
    [SerializeField]
    private float       _rateOfFire = 2.0f;
    [SerializeField]
    protected float     _bulletsSpeed = 2.0f;
    [SerializeField]
    protected int       _bulletsDamage = 1;
    [SerializeField]
    protected Layer     _bulletLayer;

    private float       _timer = 0;

    private int         _hpMax = 0;

    protected GameObject _lifeBar;

    // Use this for initialization
    public void Start()
    {
        _hpMax = _hp;
        _lifeBar = Instantiate(_lifeBarPrefab, transform);
        _lifeBar.transform.position = _lifeBarAnchor.transform.position;
        _lifeBar.transform.rotation = Quaternion.identity;
        Vector3 localScale = _lifeBar.transform.localScale;
        _lifeBar.transform.parent = _lifeBarAnchor.transform;
        _lifeBar.transform.localScale = localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetHp()
    {
        return _hp;
    }

    public void SetHp(int hp, GameObject source)
    {
        _hp = hp;
        
        _lifeBar.GetComponentInChildren<Slider>().value = (float)hp / (float)_hpMax;

        if (_hp <= 0)
        {
            OnDead(source);
            GameObject.Destroy(this.gameObject);
        }
    }

    public virtual void HitBy(GameObject sender, int bulletsDamage)
    {
        SetHp(GetHp() - bulletsDamage, sender);
    }
    
    public void TryToFire(GameObject target)
    {
        _timer += Time.deltaTime;
        if (target != null && _timer > _rateOfFire)
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * _bulletsSpeed;
            bullet.GetComponent<Bullets>().SetDamage(_bulletsDamage);
            bullet.GetComponent<Bullets>().SetIgnoreTag(gameObject.tag);
            bullet.GetComponent<Bullets>().SetSender(gameObject);
            _timer = 0;
        }
    }

    public virtual bool CanBeHit()
    {
        return true;
    }

    protected virtual void OnDead(GameObject killer)
    {
    }

    public int GetBulletLayer()
    {
        return _bulletLayer.LayerIndex;
    }
}
