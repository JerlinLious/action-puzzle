using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ctrl_bar : MonoBehaviour
{
    public GameObject bar;
    private UpdateBar barClass;

    private void Start() {
        if (bar == null) {
            P.WarningPrint(transform, this.GetType().ToString());
            return;
        }

        barClass = bar.GetComponent<UpdateBar>();
        SetBar();
    }

    public void SetBar() {
        float value = transform.GetComponent<Slider>().value;
        barClass.SetPercentValue(value);
    }
}
