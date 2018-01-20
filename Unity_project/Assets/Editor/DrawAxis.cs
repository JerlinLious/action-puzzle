using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DrawAxis : ScriptableWizard
{

    //单位向量
    private GameObject origin = null;
    public Vector2 unitVector = new Vector2(5, 5);
    public int xLength = 16, yLength = 12;
    const int MAXEDGES = 50;

    [MenuItem("Tools/DrawAxis")]
    static void CreateWizerd() {
        DisplayWizard("Draw Axis", typeof(DrawAxis), "Draw");
    }

    private GameObject bluePoint;
    private GameObject whiteLine;
    private static GameObject[,] Points = new GameObject[MAXEDGES, MAXEDGES];

    private void OnEnable() {
        P.LoadForEditor();

        bluePoint = P.Objs["bluePoint"];
        whiteLine = P.Objs["whiteLine"];

    }



    private void OnWizardCreate() {
        origin = GameObject.Find("basePoint");
        if (origin == null) {
            origin = Instantiate(P.Objs["redPoint"]);           
            origin.transform.position = Vector3.zero;
        }
        else {
            P.DestroyAllChildren(origin);

        }
        origin.name = "basePoint";

        for (int i = 0; i < MAXEDGES; i++) {
            for (int j = 0; j < MAXEDGES; j++) {
                Points[i, j] = null;
            }
        }


        if (xLength > MAXEDGES || yLength > MAXEDGES) return;

        Points[0, 0] = origin;

        for (int i = 0; i < xLength; i++) {
            for (int j = 0; j < yLength; j++) {
                if (i == 0 && j == 0) continue;

                Points[i, j] = Instantiate(bluePoint, origin.transform);
                Points[i, j].name = "P" + i.ToString() + ',' + j.ToString();

                Points[i, j].transform.position = origin.transform.position + new Vector3(i * unitVector.x, j * unitVector.y, 0);

                Transform L, D;
                //画左边
                if (i > 0) {
                    L = Instantiate(whiteLine, Points[i, j].transform).transform;
                    L.name = "left";
                    L.position = (Points[i, j].transform.position + Points[i - 1, j].transform.position) / 2;
                    L.localScale = new Vector3(unitVector.x, 1, 1);
                }
                //画下边
                if (j > 0) {
                    D = Instantiate(whiteLine, Points[i, j].transform).transform;
                    D.Rotate(new Vector3(0, 0, 90));
                    D.name = "down";
                    D.position = (Points[i, j].transform.position + Points[i, j - 1].transform.position) / 2;
                    D.localScale = new Vector3(unitVector.y, 1, 1);
                }
            }
        }
    }



}
