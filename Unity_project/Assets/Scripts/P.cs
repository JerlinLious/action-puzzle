using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个类只允许有一个实例
public class P : MonoBehaviour
{
    public static P PUBLIC;

    static UnityEngine.UI.Text debugText;
    static UnityEngine.UI.Text centralText;
    public static Dictionary<string, GameObject> Objs = new Dictionary<string, GameObject>();

    private int deathTime = 0;
    public int deathLimit = 3;
    private GameObject revivePlace = null;

    // Use this for initialization
    void Start() {
        PUBLIC = this;

        debugText = GameObject.Find("DebugText").GetComponent<UnityEngine.UI.Text>();
        centralText = GameObject.Find("CentralText").GetComponent<UnityEngine.UI.Text>();


        //Load your resources here
        LOAD("Effects/", "E_jump");
        LOAD("Effects/", "E_revive");
        LOAD("Objs/", "arrow");






        revivePlace = GameObject.Find("RevivePlace");
        if (revivePlace == null) {
            WarningPrint(transform, this.GetType().ToString(), "We need a GameObject called\"RevivePlace\" to revive the player.");
            return;
        }
    }


    public static void LOAD(string path, string name) {
        Objs[name] = (GameObject)Resources.Load(path + name);
    }

    public static void SetDebugText(string text) {
        debugText.text = text;
    }

    public static void SetCentralText(string text) {
        centralText.text = text;
    }


    public void Revive(GameObject player, float delay) {
        deathTime++;
        SetDebugText(deathTime.ToString());
        if (deathTime == deathLimit) {
            Debug.Log("deathTime==deathLimit");
        }

        player.transform.position = revivePlace.transform.position;


        GameObject E_revive = Instantiate(Objs["E_revive"]);
        E_revive.transform.position = player.transform.position;
        Destroy(E_revive, delay);

        StartCoroutine(DelayRevive(player, delay));
    }
    IEnumerator DelayRevive(GameObject player, float delay) {
        yield return new WaitForSeconds(delay);
        player.SetActive(true);
    }

    /// <summary>
    /// 用于加载编辑器扩展的资源
    /// </summary>
    static bool editorLoad = false;
    public static void LoadForEditor() {
        if (editorLoad) return;
        editorLoad = true;
        LOAD("editor/", "redPoint");
        LOAD("editor/", "bluePoint");
        LOAD("editor/", "whiteLine");
    }

    /// <summary>
    /// 销毁parent的所有子物体
    /// </summary>
    /// <param name="parent"></param>
    public static void DestroyAllChildren(GameObject parent) {
        while (parent.transform.childCount > 0) DestroyImmediate(parent.transform.GetChild(0).gameObject);
    }


    public static void WarningPrint(Transform from, string className = "", string text = "Execute disabled,some necessary properties are lost.") {
        if (className == "") className = from.GetType().ToString();
        Debug.Log("GameObject [" + GetFullPath(from) + "] =>" + className + ':' + text);

        from.gameObject.SetActive(false);
        //DestroyImmediate(from.gameObject);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    /// <summary>
    /// 获取一个物体Transform组件的完整路径，displayRoot决定是否显示根路径
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string GetFullPath(Transform t, bool displayRoot = true) {
        if (!displayRoot && t.parent == null) return "";
        string path = t.name;


        while (t.parent != null) {
            t = t.parent;
            if (!displayRoot && t.parent == null) break; 
            path = path.Insert(0, t.name + '/');
        }
        return path;
    }

}
