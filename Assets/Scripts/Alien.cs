using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Enemy
{
    [Header("Alien")]


    private GameObject _nemesis;

    public enum Type
    {
        AlienSmall, AlienBig
    }
    public enum State
    {
        Move, Fire
    }

    private Vector2 _endPosition;
    private bool _move = false;

    public void SetEndPosition(Vector2 endPosition)
    {
        _endPosition = endPosition;
        _move = true;
        GetComponent<Collider2D>().enabled = false;
    }

    void Update()
    {
        if (_move == true)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, _endPosition, 2.0f * Time.deltaTime);
            if (this.transform.position.x == _endPosition.x &&
                this.transform.position.y == _endPosition.y)
            {
                _move = false;
                GetComponent<Collider2D>().enabled = true;
                _startTime = Time.time;
            }
        }

        if (_nemesis != null)
        {
            TryToFire(_nemesis);
        }
        else
        {
            TryToFire(GameObject.FindObjectOfType<Planet>().Core);
        }
    }

    public override void HitBy(GameObject sender, int bulletsDamage)
    {
        base.HitBy(sender, bulletsDamage);

        Bullets bullet = sender.GetComponent<Bullets>();
        if (bullet != null)
        {
            _nemesis = bullet.GetSender();
        }
    }
}
