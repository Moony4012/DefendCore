using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : Entity
{
    [Header("Turret")]
    public float _fireAngle = 225.0f;

    public enum State
    {
        InvalidPlacement, ValidPlacement, Active
    }

    State _State = State.InvalidPlacement;
    void Start()
    {
        base.Start();

        _lifeBar.SetActive(false);
    }
    void Update()
    {
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, _fireAngle * -0.5f) * transform.up * 999.0f);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, _fireAngle * 0.5f) * transform.up * 999.0f);

        if (_State == State.Active)
        {
            float bestDistance = Mathf.Infinity;
            GameObject bestTarget = null;
            GameObject[] targets = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject target in targets)
            {
                Vector2 targetDir = target.transform.position - transform.position;
                float angle = Vector2.Angle(targetDir, transform.up);

                if (Mathf.Abs(angle) <= (_fireAngle * 0.5f) && (bestTarget == null || Vector2.Distance(bestTarget.transform.position,target.transform.position) < bestDistance))
                {
                    bestTarget = target;
                    bestDistance = Vector2.Distance(bestTarget.transform.position, target.transform.position);
                }
            }
            TryToFire(bestTarget);
        }
    }

    public State GetState()
    {
        return _State;
    }

    public void SetState(State newState)
    {
        switch (newState)
        {
            case State.InvalidPlacement:
                GetComponent<SpriteRenderer>().color = new Color (1.0f, 0.1194968f, 0.1194968f, 0.8823529f);
                break;
            case State.ValidPlacement:
                GetComponent<SpriteRenderer>().color = new Color(0.106f, 1, 0.071f, 0.9137255f);
                break;
            case State.Active:
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                GetComponent<Collider2D>().isTrigger = false;
                _lifeBar.transform.rotation = Quaternion.identity;
                _lifeBar.SetActive(true);
                break;
        }
        _State = newState;
    }

    public override bool CanBeHit()
    {
        if (_State == State.Active)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}