using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TorretsPlacement : MonoBehaviour
{
    // Handler Place Torrets for coins

    private GameObject Turret = null;

    int _Price;

    void Start()
    {
        
    }
    void Update()
    {
        if (Turret != null)
        {
            Vector3 Postion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Postion.z = 0;
            Turret.transform.position = Vector3.zero;
            Turret.transform.up = Postion - Turret.transform.position;
            Turret.transform.Translate(Vector3.up*3.3f, Space.Self);
            List<Collider2D> collider2Ds = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            //filter.useTriggers = true;
            filter.useLayerMask = true;
            filter.layerMask = (1 << Turret.layer);
            Debug.Log(filter.layerMask.value);
            if (Turret.GetComponent<BoxCollider2D>().OverlapCollider(filter, collider2Ds) > 0)
            {
                foreach (Collider2D collider2D in collider2Ds)
                {
                    //Debug.Log(collider2D.gameObject.name);
                }

                Turret.GetComponent<Turrets>().SetState(Turrets.State.InvalidPlacement);
            }
            else
            {
                Turret.GetComponent<Turrets>().SetState(Turrets.State.ValidPlacement);
            }

            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false && Turret.GetComponent<Turrets>().GetState() == Turrets.State.ValidPlacement)
            {
                GameObject.FindObjectOfType<GameState>().AddCoins(-_Price);
                Turret.GetComponent<Turrets>().SetState(Turrets.State.Active);
                Turret = null;
            }
            else if(Input.GetMouseButtonDown(1))
            {
                GameObject.Destroy(Turret);
                Turret = null;
            }
        }
    }

    public void BeginPlace(TurretsData data)
    {
        Turret = GameObject.Instantiate(data._prefab);
        Turret.GetComponent<Turrets>().SetState(Turrets.State.InvalidPlacement);

        _Price = data._price;
    }
}
