using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatonEditorMag : MonoBehaviour {

    public GameObject m_addBtn;

    public GameObject m_closeBtn;

    public GameObject beantoneditorPrefab;

    public UIGrid grid;

    private List<SingleBeatonDataUI> beatonUIList = new List<SingleBeatonDataUI>();
	// Use this for initialization
	void Start () {

        UIEventListener.Get(m_addBtn).onClick += OnAddOneBeaton;

        UIEventListener.Get(m_closeBtn).onClick += OnClosePanel;

	}

    void OnClosePanel(GameObject go)
    {
        gameObject.SetActive(false);
    }
    void OnAddOneBeaton(GameObject go)
    {
        SingleBeatonDataUI beatonSetUI = NGUITools.AddChild(grid.gameObject, beantoneditorPrefab).GetComponent<SingleBeatonDataUI>();
        beatonSetUI.initdata(new BeatonData());
        beatonUIList.Add(beatonSetUI);

        grid.Reposition();

    }

    public void deleteBeatonUI(GameObject deleteChild)
    {
        SingleBeatonDataUI chidl = deleteChild.GetComponent<SingleBeatonDataUI>();
        beatonUIList.Remove(chidl);

        Destroy(deleteChild);
        grid.Reposition();
    }
    public void initView(List<BeatonData> datasList)
    {
        while (grid.transform.childCount > 0)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }

        //foreach (SingleBeatonDataUI child in beatonUIList)
        //{
        //    grid.RemoveChild(child.transform);
        //    Destroy(child.gameObject);
        //}
        beatonUIList.Clear();
        grid.transform.localPosition = new Vector3(0, 58, 0);
        for (int i = 0; i < datasList.Count; i++)
        {
            SingleBeatonDataUI beatonSetUI = NGUITools.AddChild(grid.gameObject, beantoneditorPrefab).GetComponent<SingleBeatonDataUI>();
            beatonSetUI.initdata(datasList[i]);
            beatonUIList.Add(beatonSetUI);
           // skillitems[i] = skillitem;
            //skillitem.UpdateEnabled();
        }

        grid.Reposition();
    }

    public List<BeatonData> getbeatonData()
    {
        List<BeatonData> datas = new List<BeatonData>();
        for (int i = 0; i < beatonUIList.Count; i++)
        {
            datas.Add(beatonUIList[i].getData());
        }

        return datas;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
