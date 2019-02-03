using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubInfoManager : MonoBehaviour
{
    public Animation CalenderAnim;
    public Animation CPIAnim;
    
    public TextAnim DayText;
    public TextAnim MonText;
    public TextAnim YearText;

    public TextAnim CPIText;

    bool isDisplay;

    private void Awake()
    {
        DisplaySub(false);
        var calState = CalenderAnim["CalenderOut"];
        calState.weight = 1;
        calState.enabled = true;
        calState.normalizedTime = 1;
        var cpiState = CPIAnim["CPIOut"];
        cpiState.weight = 1;
        cpiState.enabled = true;
        cpiState.normalizedTime = 1;

        CalenderAnim.Sample();
        CPIAnim.Sample();
    }

    internal void DisplaySub(bool isIn)
    {
        if (isDisplay == isIn)
            return;

        CalenderAnim.CrossFade(isIn ? "CalenderIn" : "CalenderOut");
        CPIAnim.CrossFade(isIn ? "CPIIn" : "CPIOut");

        isDisplay = isIn;
    }
}
