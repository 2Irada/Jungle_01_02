using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class JellyShooter : MonoBehaviour
{
    private bool worldLockJelly = false;
    private bool worldLockArm = false;

    #region MouseCursor
    public Texture2D cursurIconBasic;
    public Texture2D cursurIconEye;
    public Texture2D cursurIconSlime;
    public Texture2D cursurIconEyeSlime;

    private Vector2 hotSpot;
    #endregion

    public JellyData data;
    public JellyBullet jellyBullet;
    public GameObject slimeHeadGraphic;

    public ArmShoot armShoot;
    public GameObject basicArm;
    public GameObject slimeEye;
    public bool nowJellyEffect = false;

    public Coloring jellyColoring = Coloring.Red;

    public ColoredObject jelliedObject = null;
    public ColoredObject eyeObject = null;

    public bool canShootJelly = true;
    public bool canRetrieveJelly = false;

    public bool isEyeGet = false;

    public bool canShootArm = true;

    private void Start()
    {
        
        //int alpha = int.Parse(str);
        switch(UIManager.instance.sceneNum)
        {
            case 1:
                worldLockArm = false;
                worldLockJelly = false;
                break;
            case 2:
                SetCursorIcon(1);
                worldLockArm = false;
                worldLockJelly = true;
                break;
            case 3:
                worldLockArm = true;
                worldLockJelly = true;
                break;
            default:
                SetCursorIcon(default);
                worldLockArm = false;
                worldLockJelly = false;
                break;
        }
        UpdateHeadColor();
        canShootJelly = true;
        canRetrieveJelly = false;

        canShootArm = true;

        isEyeGet = false;

        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && worldLockJelly)
        {
            IsJellyBlock(jelliedObject);
        }

        else if (Input.GetMouseButtonDown(1) && worldLockArm)
        {
            var _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (_hit.collider != null)
            {
                ColoredObject _obj = _hit.collider.GetComponent<ColoredObject>();
                if (_obj != null && !_obj.isEyeball && isEyeGet && !_obj._isJellied)
                {
                    eyeObject = _obj;
                    if (canShootArm && !nowJellyEffect) ShootArmSetEye(_obj.transform);
                }

                else if (_obj != null && _obj.isEyeball && !isEyeGet && !_obj._isJellied)
                {
                    eyeObject = _obj;
                    if (canShootArm && !nowJellyEffect) ShootArmGetEye(_obj.transform);
                }
            }
        }
    }

    private void SetCursorIcon(int cursurNum)
    {
        hotSpot.x = cursurIconBasic.width / 2;
        hotSpot.y = cursurIconBasic.height / 2;
        switch (cursurNum)
        {
            case 0:
                Debug.Log("AimBasic");
                Cursor.SetCursor(cursurIconBasic, hotSpot, CursorMode.Auto);
                break; 
            case 1:
                Cursor.SetCursor(cursurIconSlime, hotSpot, CursorMode.Auto);
                break; 
            case 2:
                Cursor.SetCursor(cursurIconEye, hotSpot, CursorMode.Auto);
                break;
            case 3:
                Cursor.SetCursor(cursurIconEyeSlime, hotSpot, CursorMode.Auto);
                break;
            default:
                Debug.Log("default Cursor");
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }
        //
    }

    private void IsJellyBlock(ColoredObject _jelliedObject)
    {
        if (_jelliedObject == null)
        {
            CheckShoot();
        }
        else if (_jelliedObject != null)
        {
            if (canRetrieveJelly) RetriveJelly();
        }
    }

    private void CheckShoot()
    {
        var _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        ShootJelly(_hit);
    }

    private void ShootJelly(RaycastHit2D _hit)
    {
        if (_hit.collider != null)
        {
            ColoredObject _obj = _hit.collider.GetComponent<ColoredObject>();
            
            if (_obj != null && _obj.isEyeball && canShootArm)
            {
                jelliedObject = _obj;
                if (canShootJelly)
                {
                    ShootJelly(_obj.transform);
                    jelliedObject._isJellied = true;
                }
            }
        }
        else if (jelliedObject != null)
        {
            if (canRetrieveJelly) RetriveJelly();
        }
    }

    private void ShootArmSetEye(Transform target)
    {
        canShootArm = false;

        basicArm.SetActive(false);
        armShoot.transform.position = basicArm.transform.position;
        armShoot.SetTarget(target, false);
        isEyeGet = false;
        armShoot.gameObject.SetActive(true);
        
    }


    private void ShootArmGetEye(Transform target)
    {
        canShootArm = false;

        basicArm.SetActive(false);
        armShoot.transform.position = basicArm.transform.position;
        armShoot.SetTarget(target, false);
        isEyeGet = true;
        armShoot.gameObject.SetActive(true);
   
    }

    public void EyeEffect()
    {
        if (!eyeObject.isEyeball)
        {
            eyeObject.EyeballGet();
            eyeObject.isEyeball = true;

            if (canShootJelly) SetCursorIcon(1);
            else SetCursorIcon(0);
        }
        else
        {
            eyeObject.EyeballEaten();
            eyeObject.isEyeball = false;

            if (canShootJelly) SetCursorIcon(3);
            else SetCursorIcon(2);
            
        }
        eyeObject.UpdateColoringLogic();
    }

    private void ShootJelly(Transform target)
    {
        if (isEyeGet)
        {
            SetCursorIcon(2);
        }
        else
        {
            SetCursorIcon(0);
        }

        canShootJelly = false;
        slimeHeadGraphic.SetActive(false);
        jellyBullet.transform.position = slimeHeadGraphic.transform.position;
        jellyBullet.SetTarget(target, false);
        jellyBullet.GetComponent<SpriteRenderer>().color = ColorManager.instance.GetColorByColoring(jellyColoring);
        jellyBullet.gameObject.SetActive(true);
    }

    private void RetriveJelly()
    {
        if (isEyeGet)
        {
            SetCursorIcon(3);
        }
        else
        {
            SetCursorIcon(1);
        }
        

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
