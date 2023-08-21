using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColoredObject : MonoBehaviour, ISerializationCallbackReceiver
{
    public Coloring objectColoring = new Coloring();
    [HideInInspector] public Coloring currentColoring = new Coloring();
    public GameObject eyeballObject;
    public GameObject glasses;
    public bool startAsEyeball;
    public bool isEyeball;
    [HideInInspector] public bool isTriggerActive;
    
    private bool isDownOn;
    //public bool isDownSpike;
    [SerializeField] private bool isChangeBlock;

    public bool _isJellied = false;
    private List<SpriteRenderer> _eyeballRenderers = new List<SpriteRenderer>();
    private bool _didEyeBallInit = false;
    
    //REFERENCES
    private LineRenderer _lr;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        _collider.enabled = true;
        InitializeColoring();
        if (_didEyeBallInit == false) InitEyeball();
    }

    private void OnEnable()
    {
        FindObjectOfType<ColorManager>().mainColoringChanged += UpdateColoringLogic;
    }

    private void OnDisable()
    {
        
        ColorManager.instance.mainColoringChanged -= UpdateColoringLogic;
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (PrefabUtility.GetPrefabInstanceStatus(this) == PrefabInstanceStatus.Connected)
        {
            if (TryGetComponent<SpriteRenderer>(out var _sr))
            {
                if (FindObjectOfType<ColorManager>() != null)
                {
                    _sr.color = FindObjectOfType<ColorManager>().GetColorByColoring(objectColoring);
                }
            }
            eyeballObject.SetActive(true);
            _eyeballRenderers.Clear();
            _eyeballRenderers.AddRange(eyeballObject.GetComponentsInChildren<SpriteRenderer>());

            if (startAsEyeball)
            {
                eyeballObject.SetActive(true);
                isEyeball = true;
            }
            else
            {
                eyeballObject.SetActive(false);
                isEyeball = false;
            }
        }
#endif
    }

    public void OnAfterDeserialize()
    {
    }


    /// <summary>
    /// 오브젝트의 원본 색을 설정.
    /// </summary>
    private void InitializeColoring()
    {
        _spriteRenderer.color = ColorManager.instance.GetColorByColoring(objectColoring);
        UpdateColoringLogic();
    }

    /// <summary>
    /// 이 오브젝트를 젤리가 뒤덮음.
    /// </summary>
    /// <param name="jellyColoring"></param>
    public void GetJellied(Coloring jellyColoring)
    {
        _isJellied = true;
        UpdateColoringLogic();
    }

    public void GetUnjellied()
    {
        //필요: 젤리 없어지는 시각 효과
        _isJellied = false;
        UpdateColoringLogic();
    }

    /// <summary>
    /// 메인 컬러링과 이 오브젝트의 컬러링을 비교한 뒤 콜라이더 등 설정.
    /// </summary>
    public void UpdateColoringLogic()
    {
        if (isChangeBlock) {
            glasses.gameObject.SetActive(true);
            currentColoring = _isJellied ? FindObjectOfType<JellyShooter>().jellyColoring : ColorManager.instance.mainColoring;
        }
        else {
            currentColoring = _isJellied ? FindObjectOfType<JellyShooter>().jellyColoring : objectColoring;
        }

        if (ColorManager.instance.mainColoring != currentColoring) //색 다를 때
        {
            if (tag == "Spike" || tag =="Item")
            {
                isTriggerActive = true;
            }
            else if (tag == "Ground")
            {
                _collider.isTrigger = false;
            }


            if (_isJellied)
            {
                SetActiveLineRenderer(true);
                _spriteRenderer.enabled = false;
            }
            else
            {
                SetActiveLineRenderer(false);
                _spriteRenderer.enabled = true;
            }

            
        }
        else //색 같을때
        {
            if (tag == "Spike" || tag == "Item")
            {
                isTriggerActive = false;
            }
            else if (tag == "Ground")
            {
                _collider.isTrigger = true;
            }


            SetActiveLineRenderer(true);
            _spriteRenderer.enabled = false;
            
        }

        if (isEyeball)
        {
            if (ColorManager.instance.mainColoring == currentColoring || _isJellied) //젤리 붙었거나 결론적으로 투명하거나 둘중 하나면 반투명
            {
                SetEyeballAlpha(0.5f);
            }
            else 
            { 
                SetEyeballAlpha(1.0f);
            }
        }
    }

    private void SetActiveLineRenderer(bool value)
    {
        _lr.startColor = ColorManager.instance.GetHighlightColorByColoring(objectColoring);
        _lr.endColor = ColorManager.instance.GetHighlightColorByColoring(objectColoring);
        _lr.enabled = value;
    }

    public void InitEyeball()
    {
        _didEyeBallInit = true;
        eyeballObject.SetActive(true);
        _eyeballRenderers.Clear();
        _eyeballRenderers.AddRange(eyeballObject.GetComponentsInChildren<SpriteRenderer>());

        if (startAsEyeball)
        {
            eyeballObject.SetActive(true);
            isEyeball = true;
        }
        else 
        {
            eyeballObject.SetActive(false);
            isEyeball = false;
        }
    }

    public void EyeballEaten()
    {
        if (!isEyeball) return;
        //눈깔 없애기
        eyeballObject.SetActive(false);
    }
    public void EyeballGet()
    {
        if (isEyeball) return;
        //눈깔 없애기
        eyeballObject.SetActive(true);
    }


    public void JellyLeavesEyeball()
    {
        if (isEyeball == false) return;

        isEyeball = false;
    }

    private void SetEyeballAlpha(float alpha)
    {
        _eyeballRenderers.Clear();
        _eyeballRenderers.AddRange(eyeballObject.GetComponentsInChildren<SpriteRenderer>());

        for (int i = 0; i < _eyeballRenderers.Count; i++)
        {
            Color _color = _eyeballRenderers[i].color;
            _eyeballRenderers[i].color = new Color(_color.r, _color.g, _color.b, alpha);
        }
    }
}