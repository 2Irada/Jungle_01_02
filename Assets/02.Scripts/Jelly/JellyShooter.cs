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
    public GameObject basicArm;
    public GameObject slimeEye;

    public Coloring jellyColoring = Coloring.Red;

    public ColoredObject jelliedObject = null;
    public ColoredObject eyeObject = null;

    public bool canShootJelly = true;    
    public bool canRetrieveJelly = false;

    public bool isEyeGet = false;

    public bool canShootArm = true;

    private void Start()
    {
        UpdateHeadColor();
        canShootJelly = true;
        canRetrieveJelly = false;
        
        canShootArm = true;

        isEyeGet = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsJellyBlock();
        }

        else if(Input.GetMouseButtonDown(1))
        {
            var _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            
            if(_hit.collider != null)
            {
                ColoredObject _obj = _hit.collider.GetComponent<ColoredObject>();
                if (_obj != null && !_obj.isEyeball && isEyeGet)
                {
                    eyeObject = _obj;
                    ShootArmSetEye();
                    Debug.Log("SetArm");
                }

                else if (_obj != null && _obj.isEyeball && !isEyeGet)
                {
                    eyeObject = _obj;
                    ShootArmGetEye();
                    Debug.Log("GetArm");
                }
            }
        }
    }

    private void IsJellyBlock()
    {
        if (jelliedObject == null)
        {
            CheckShoot();
        }
        else if (jelliedObject != null)
        {
            if (canRetrieveJelly) RetriveJelly();
        }
    }

    private void CheckShoot()
    {
        var _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (_hit.collider != null)
        {
            ColoredObject _obj = _hit.collider.GetComponent<ColoredObject>();
            if (_obj != null && _obj.isEyeball)
            {
                if (canShootJelly) ShootJelly(_obj.transform);
            }
        }
        else if (jelliedObject != null)
        {
            if (canRetrieveJelly) RetriveJelly();
        }
    }

    private void ShootArmSetEye()
    {
        if (!eyeObject.isEyeball)
        {
            eyeObject.EyeballGet();
            //jelliedObject = null;
            eyeObject.isEyeball = true;
            eyeObject.UpdateColoringLogic();
        }

        isEyeGet = false;
        slimeEye.SetActive(false);
        //basicArm.SetActive(false);       
    }


    private void ShootArmGetEye()
    {
        if (eyeObject.isEyeball)
        {
            eyeObject.EyeballEaten();
            //jelliedObject = null;
            eyeObject.isEyeball = false;
            eyeObject.UpdateColoringLogic();
        }

        isEyeGet = true;
        slimeEye.SetActive(true);
        //basicArm.SetActive(false);
    }



    private void ShootJelly(Transform target)
    {
        canShootJelly = false;
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

        canRetrieveJelly = false;
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
        canRetrieveJelly = true;
    }

    public void UnjellifyComplete()
    {
        jelliedObject.GetUnjellied();
        jelliedObject = null;
    }

    public void SetJellyColoring(Coloring coloring)
    {
        Debug.Log(coloring);
        jellyColoring = coloring;
        UpdateHeadColor();
    }
}
