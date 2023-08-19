using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private Canvas canvas;

    #region UI Prefab
    // [SerializeField] private GameObject ColorInfoChildPref;    
    [SerializeField] private GameObject GameOverPref;
    [SerializeField] private GameObject XBtnPref;
    [SerializeField] private int maxColorCount;
    #endregion

    public bool _isGameEnd;

    [SerializeField] private Image[] ColorList;

    public GameObject gameOver;
    private TextMeshProUGUI ingKu;

    private bool plusIngku;
    public static int deathCount;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        ColorList[0].color = ColorManager.instance.red;
        ColorList[1].color = ColorManager.instance.green;
        ColorList[2].color = ColorManager.instance.blue;

        UIInfoCreate();       
    }

    private void OnEnable()
    {
        plusIngku = false;       
    }

    private void Update()
    {
        if (_isGameEnd && !plusIngku)
        {
            StopAllCoroutines();
            StartCoroutine(StartIngKu());
        }
    }

    

    void UIInfoCreate()
    {    
       // ColorInfoCreate();
        SpawnXBtn();
        GameOverCerate();
    }

    /*
    void ColorInfoCreate()
    {
        for (int i = 0; i < maxColorCount; i++)
        {
            GameObject colorInfo = Instantiate(ColorInfoChildPref, canvas.transform);

            TextMeshProUGUI objText = GetComponentInChildren<TextMeshProUGUI>();

            Image objColor = colorInfo.GetComponent<Image>();
            objColor.color = Color.red;
            renderer = objColor.GetComponent<MeshRenderer>();
            Vector2 colorInfoSize = renderer.bounds.size;

            colorInfo.transform.position = new Vector2(colorInfo.transform.position.x + (colorInfoSize.x * i) + 5, colorInfo.transform.position.y);

            objText.text = (i+1).ToString();
        }
    }
    */
    void GameOverCerate()
    { // gameOver Prefab에 있는 TMPro를 가져옴. 만약 TMPro가 추가된다면 코드 수정해야 함
        gameOver = Instantiate(GameOverPref, canvas.transform);
        ingKu = gameOver.GetComponentInChildren<TextMeshProUGUI>();
        ingKu.color = new Color(255, 255, 255, 0);
        gameOver.SetActive(false);
    }

    void SpawnXBtn()
    {
        Instantiate(XBtnPref, canvas.transform);
        Button XButtonEvent = XBtnPref.GetComponent<Button>();
        XButtonEvent.onClick.AddListener(ExitGame);
    }

    public IEnumerator StartIngKu()
    {
        SceneController.instance.isRestart = true;
        if (!gameOver.activeSelf) gameOver.SetActive(true);

        plusIngku = true;
        deathCount++;
        string forIngKu = ingKu.text;
        for (int i = 1; i < deathCount; i++)
        {
            forIngKu += "ㅋ";
        }
        ingKu.text = forIngKu;
        ingKu.color = new Color(255, 255, 255, 255);

        yield return new WaitForSeconds(2.0f);
        plusIngku = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // 조정 후 세이브 포인트로 스폰하도록 변경
    }

    public void GameReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
