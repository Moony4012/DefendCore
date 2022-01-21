using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    // Bullets: collide with opponent only and remove HP from Turret or Planete and avoid shoot the plannet
    private int _bulletsDamage;
    private string _ignoreTag;
    private GameObject _sender;

    [SerializeField]
    private GameObject _holePrefab;

    [SerializeField]
    private float _holeSize = 0.15f;

    private List<Collider2D> _holeList = new List<Collider2D>();
    private PolygonCollider2D _collider = null;

    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (_holeList.Count > 0)
        {
            foreach (Vector2 point in _collider.points)
            {
                Vector3 point3D = new Vector3(point.x, point.y, 0.0f);
                Vector3 worldPoint3D = transform.position + (transform.rotation * Vector3.Scale(point3D, transform.localScale));
                Vector2 worldPoint = new Vector2(worldPoint3D.x, worldPoint3D.y);
                Debug.DrawLine(worldPoint, GameObject.FindObjectOfType<Planet>().transform.position);
                
                bool isInHolde = false;
                foreach (Collider2D holeCollider in _holeList)
                {
                    if (holeCollider.OverlapPoint(worldPoint) == true)
                    {
                        isInHolde = true;
                        break;
                    }
                }

                if (isInHolde == false && GameObject.FindObjectOfType<Planet>().GetComponent<Collider2D>().OverlapPoint(worldPoint) == true)
                {
                    CreateHole(worldPoint);
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedGameObject = collision.collider.gameObject;

        if (collidedGameObject.tag != _ignoreTag && collidedGameObject.tag != "bullet")
        {
            Entity entity = collidedGameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if (entity.CanBeHit() == true)
                {
                    entity.HitBy(this.gameObject, _bulletsDamage);
                }
            }
            else
            {
                Planet planet = collidedGameObject.GetComponent<Planet>();
                if (planet != null)
                {
                    CreateHole(collision.contacts[0].point);
                }
            }
            GameObject.Destroy(this.gameObject);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collidedGameObject = collider.gameObject;
        if (collidedGameObject.tag == "hole")
        {
            _holeList.Add(collider);
            Physics2D.IgnoreCollision(_collider, GameObject.FindObjectOfType<Planet>().GetComponent<Collider2D>());
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GameObject collidedGameObject = collider.gameObject;
        if (collidedGameObject.tag == "hole")
        {
            _holeList.Remove(collider);
            if (_holeList.Count == 0)
            {
                Physics2D.IgnoreCollision(_collider, GameObject.FindObjectOfType<Planet>().GetComponent<Collider2D>(), false);
            }
        }
    }

    void CreateHole(Vector2 position)
    {
        GameObject hole = GameObject.Instantiate(_holePrefab, position, Quaternion.identity);
        hole.transform.localScale = Vector3.one * _holeSize;
        GameObject.Destroy(this.gameObject);
    }
    public void SetDamage(int bulletsDamage)
    {
        _bulletsDamage = bulletsDamage;
    }

    public void SetIgnoreTag(string tag)
    {
        _ignoreTag = tag;
    }

    public GameObject GetSender()
    {
        return _sender;
    }

    public void SetSender(GameObject sender)
    {
        _sender = sender;
        Entity entity = _sender.GetComponent<Entity>();
        if (entity != null)
        {
            this.gameObject.layer = entity.GetBulletLayer();
        }

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _sender.GetComponent<Collider2D>());
    }
}
