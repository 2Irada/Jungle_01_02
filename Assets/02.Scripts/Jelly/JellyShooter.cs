using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class JellyShooter : MonoBehaviour
{
    public JellyData data;
    public JellyBullet jellyBullet;
    public GameObject slimeHeadGraphic;

    public Coloring jellyColoring = Coloring.Red;

    [HideInInspector] public ColoredObject jelliedObject = null;
    public bool canShoot = true;
    public bool canRetrieve = false;

    private void Start()
    {
        UpdateHeadColor();
        canShoot = true;
        canRetrieve = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (jelliedObject == null)
            {
                var _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (_hit.collider != null)
                {
                    ColoredObject _obj = _hit.collider.GetComponent<ColoredObject>();
                    if (_obj != null && _obj.isEyeball)
                    {
                        if (canShoot) ShootJelly(_obj.transform);
                    }
                }
            }

            else if(jelliedObject != null)
            {
                if (canRetrieve) RetriveJelly();
            }
        }
    }

    private void ShootJelly(Transform target)
    {
        canShoot = false;
        slimeHeadGraphic.SetActive(false);
        jellyBullet.transform.position = slimeHeadGraphic.transform.position;
        jellyBullet.SetTarget(target, false);
        jellyBullet.GetComponent<SpriteRenderer>().color = ColorManager.instance.GetColorByColoring(jellyColoring);
        jellyBullet.gameObject.SetActive(true);
    }

    private void RetriveJelly()
    {
        if (jelliedObject.isEyeball)
        {
            SetJellyColoring(jelliedObject.objectColoring);
            jelliedObject.JellyLeavesEyeball();
        }

        canRetrieve = false;
        jellyBullet.transform.position = jelliedObject.transform.position;
        jellyBullet.SetTarget(slimeHeadGraphic.transform, true);
        jellyBullet.GetComponent<SpriteRenderer>().color = ColorManager.instance.GetColorByColoring(jellyColoring);
        jellyBullet.gameObject.SetActive(true);

        Collider2D _col = jelliedObject.GetComponent<Collider2D>();
        bool _tempActive = _col.enabled;
        bool _tempTrigger = _col.isTrigger;
        _col.enabled = true;
        _col.isTrigger = true;
        Vector2 _closestPoint = _col.ClosestPoint(slimeHeadGraphic.transform.position);
        _col.enabled = _tempActive;
        _col.isTrigger = _tempTrigger;
        Vector2 _direction = ((Vector2)slimeHeadGraphic.transform.position - _closestPoint).normalized;
        GetComponent<JellyEffect>().JellyEffectOff(_closestPoint + _direction * 0.3f);

    }

    private void UpdateHeadColor()
    {
        slimeHeadGraphic.GetComponent<SpriteRenderer>().color = ColorManager.instance.GetColorByColoring(jellyColoring);
    }

    public void JellifyComplete(ColoredObject jelliedObject)
    {
        jelliedObject.GetJellied(jellyColoring);
        this.jelliedObject = jelliedObject;
        canRetrieve = true;
    }

    public void UnjellifyComplete()
    {
        jelliedObject.GetUnjellied();
        jelliedObject = null;
    }

    public void SetJellyColoring(Coloring coloring)
    {
        jellyColoring = coloring;
        UpdateHeadColor();
    }
}
