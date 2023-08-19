using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetJellyEffectMask : MonoBehaviour
{
    [SerializeField]private GameObject prefabMask;
    [SerializeField] private float margin = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject _temp = new GameObject();
        SpriteMask _mask = _temp.AddComponent<SpriteMask>();

        _mask.sprite = GetComponent<SpriteRenderer>().sprite;
        _mask.enabled = false;
        _temp.transform.position = transform.position;
        _temp.name = "Platform_Mask";
        _temp.transform.localScale = new Vector3(transform.localScale.x + margin, transform.localScale.y + margin, transform.localScale.z);
        _temp.transform.parent = this.transform;
    }
}
