using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSkill : MonoBehaviour
{
    private Image m_image;
    private Text restTimeText;
    private float restTime = 0f;

    public float coldDownTime = 5f;
    public bool restTimeDisplay = true;
    public bool isCd = false;

    // Use this for initialization
    void Start() {
        m_image = transform.Find("mask").GetComponent<Image>();
        restTimeText = transform.Find("restTimeText").GetComponent<Text>();
    }

    private void Update() {
        if (restTime > 0.0001f) {
            restTime -= Time.deltaTime;
            isCd = true;

        }
        else isCd = false;

        if (restTime > coldDownTime) restTime = coldDownTime;
        m_image.fillAmount = restTime / coldDownTime;

        if(restTimeDisplay) restTimeText.text = isCd ? restTime.ToString("0.0") : "";       
    }

    //重置CD
    public void ResetCd() {
        restTime = coldDownTime;
    }
}
