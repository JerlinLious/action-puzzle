using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBar : MonoBehaviour
{


    private float value = 1f;          //百分比值
    public float Xmax = 393f;
    public bool byMaskMove = true;     //值为true则控制mask移动

    public bool usePercentage = false;
    public bool useSlash = false;
    public float maxSlashValue = 100;
    public float currSlashValue = 100;

    private Animator m_animator;

    private Transform mask, fill;
    private UnityEngine.UI.Text valueText;

    private void Start() {
        if (m_animator != null) m_animator = GetComponent<Animator>();

        mask = transform.Find("mask");
        fill = mask.Find("fill");
        valueText = transform.Find("valueText").GetComponent<UnityEngine.UI.Text>();

        SetBar();
    }

    private void Update() {


        SetBar();

        if (m_animator != null) m_animator.SetFloat("value", value);
    }

    private void SetBar() {
        value = currSlashValue / maxSlashValue;

        if (value < 0f) value = 0f;
        if (value > 1f) value = 1f;

        float posX = (1 - value) * Xmax;

        if (usePercentage) {
            valueText.text = currSlashValue.ToString("0") + "%";
        }
        else if (useSlash) {
            valueText.text = currSlashValue.ToString("0") + "/" + maxSlashValue.ToString("0");
        }

        if (byMaskMove) {
            mask.localPosition = new Vector3(-posX, mask.localPosition.y, mask.localPosition.z);
            fill.localPosition = new Vector3(posX, fill.localPosition.y, fill.localPosition.z);
        }
        else {
            fill.localPosition = new Vector3(-posX, fill.localPosition.y, fill.localPosition.z);
        }
    }

    /*
    private string ValueFormat(char[] c) {
        int i = 0;
        while (i < c.Length - 1 && c[i] == '0') {
            c[i] = ' ';
            i++;
        }
        return new string(c);
    }*/

    /// <summary>
    /// 设置当前进度为指定的百分比[0,1]
    /// </summary>
    /// <param name="x"></param>
    public void SetPercentValue(float x) {
        currSlashValue = x * maxSlashValue;
    }

    /// <summary>
    /// 按百分比增加进度
    /// </summary>
    /// <param name="x"></param>
    public void AddPercentValue(float x) {
        currSlashValue += x * maxSlashValue;
    }
}
