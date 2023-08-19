using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    [HideInInspector] public Coloring mainColoring = Coloring.Black;
    
    [Header("Colors")]
    public Color black;
    public Color red;
    public Color green;
    public Color blue;
    public Color blackHighlight;
    public Color redHighlight;
    public Color greenHighlight;
    public Color blueHighlight;

    public delegate void MainColoringChanged();
    public MainColoringChanged mainColoringChanged;

    #region Monobehavior Methods
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //디버그용
        if (Input.GetKeyDown(KeyCode.Space))
        {
        //    AutoSwitchMainColoring();
        }
    }
    #endregion

    /// <summary>
    /// 메인 컬러링을 변경. 메인 컬러링은 이 함수를 통해서만 변경할 것.
    /// </summary>
    /// <param name="targetColoring"></param>
    public void SwitchMainColoring(Coloring targetColoring)
    {
        mainColoring = targetColoring;
        Camera.main.backgroundColor = GetColorByColoring(targetColoring);
        mainColoringChanged?.Invoke();
    }

    /// <summary>
    /// 다음 색으로 메인 컬리링을 변경
    /// </summary>
    public void AutoSwitchMainColoring()
    {
        if (mainColoring == Coloring.Black) { SwitchMainColoring(Coloring.Red); }
        else if (mainColoring == Coloring.Red) { SwitchMainColoring(Coloring.Green); }
        else if (mainColoring == Coloring.Green) { SwitchMainColoring(Coloring.Blue); }
        else if (mainColoring == Coloring.Blue) { SwitchMainColoring(Coloring.Red); }
    }

    /// <summary>
    /// 컬러링에 대응되는 실제 Color 값을 가져옴.
    /// </summary>
    /// <param name="coloring"></param>
    /// <returns></returns>
    public Color GetColorByColoring(Coloring coloring)
    {
        switch (coloring)
        {
            case Coloring.Black:
                return black;
            case Coloring.Red:
                return red;
            case Coloring.Green:
                return green;
            case Coloring.Blue:
                return blue;
            default:
                Debug.LogError("Coloring out of bounds.");
                return black;
        }
    }

    public Color GetHighlightColorByColoring(Coloring coloring)
    {
        switch (coloring)
        {
            case Coloring.Black:
                return blackHighlight;
            case Coloring.Red:
                return redHighlight;
            case Coloring.Green:
                return greenHighlight;
            case Coloring.Blue:
                return blueHighlight;
            default:
                Debug.LogError("Coloring out of bounds.");
                return black;
        }
    }
}

public enum Coloring
{
    Black,
    Red,
    Green,
    Blue
}
