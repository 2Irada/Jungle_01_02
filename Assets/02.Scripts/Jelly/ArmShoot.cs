using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArmShoot : MonoBehaviour
{
    [SerializeField] private GameObject eyeGetObj;
    public JellyData data;
    public bool isReturning;
    private Transform _target;
    [SerializeField] private JellyShooter _shooter;
    private bool isEyeEffectOn;

    public void SetTarget(Transform target, bool isReturning)
    {
        _target = target;
        this.isReturning = isReturning;
    }

    private void Awake()
    {
        eyeGetObj.gameObject.SetActive(false);
        _shooter = FindObjectOfType<JellyShooter>();
    }

    private void OnEnable()
    {
        if(!_shooter.isEyeGet)
        {
            eyeGetObj.SetActive(true);
            _shooter.slimeEye.SetActive(false);
        }

        else
        {
            eyeGetObj.SetActive(false);
        }
        isEyeEffectOn = false;
        isReturning = false;
    }

    // Start is called before the first frame update
    private void Update()
    {
        if (!isReturning && Vector2.Distance(transform.position, _target.position) > data.jellyBulletSpeed * Time.deltaTime * 2f)
        {            
            Vector2 direction = (_target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.position += (Vector3)direction * data.jellyBulletSpeed * Time.deltaTime;
        }

        else
        {
            if (_shooter.isEyeGet) 
            { 
                eyeGetObj.SetActive(true); 
            }

            else eyeGetObj.SetActive(false);


            if (!isEyeEffectOn) _shooter.EyeEffect();
            isEyeEffectOn = true;
            isReturning = true;
        }


        if (isReturning)
        {
            Vector2 direction = (_shooter.basicArm.transform.position - transform.position).normalized;
            transform.position += (Vector3)direction * data.jellyBulletSpeed * Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isReturning && collision.gameObject.CompareTag("Eye"))
        {
            
            isReturning = false;
            if(_shooter.isEyeGet)
            {               
                _shooter.slimeEye.SetActive(true);
            }

            _shooter.basicArm.SetActive(true);
            _shooter.canShootArm = true;
            gameObject.SetActive(false);
        }
    }
}
