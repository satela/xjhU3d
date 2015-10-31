using UnityEngine;
using System.Collections;

enum Button_Tpe
{
    BTN_INVALID = -1,
    BTN_ACCEPT,
    BTN_CANCEL,
    BTN_OK,

    BTN_MAX

}
public class BarNpc : Npc {

    public static BarNpc _instance;
    public TweenPosition questTween;
    public GameObject closebtn;

    [HideInInspector]
    public bool isInTask = false;
    [HideInInspector]
    public int killCount = 0;
    public GameObject[] allButtons = new GameObject[(int)Button_Tpe.BTN_MAX];
    public UILabel deslbl;

    private PlayerStatus status;

    public void Awake()
    {
        _instance = this;
    }


	// Use this for initialization
	void Start () {

        initButtons();
        UIEventListener.Get(closebtn).onClick += OnCloseButton;
        status = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
	}
	
    void initButtons()
    {
        for(int i=0;i < allButtons.Length;i++)
        {
            UIEventListener.Get(allButtons[i]).onClick += onClickButton;
        }
    }

    void showTask()
    {
        deslbl.text = "任务：\n杀死10只狼\n\n奖励：\n1000金币";
        allButtons[(int)Button_Tpe.BTN_OK].SetActive(false);
        allButtons[(int)Button_Tpe.BTN_ACCEPT].SetActive(true);
        allButtons[(int)Button_Tpe.BTN_CANCEL].SetActive(true);
    }

    void showProgress()
    {
        deslbl.text = "任务：\n您已经杀死了" + killCount + "/10只狼\n\n奖励：\n1000金币";
        allButtons[(int)Button_Tpe.BTN_OK].SetActive(true);
        allButtons[(int)Button_Tpe.BTN_ACCEPT].SetActive(false);
        allButtons[(int)Button_Tpe.BTN_CANCEL].SetActive(false);
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null)
        {
            ShowQuest();
            if (!isInTask)
                showTask();
            else
                showProgress();
        }
    }
    void ShowQuest()
    {
        questTween.gameObject.SetActive(true);
        questTween.PlayForward();
    }

    public void OnCloseButton(GameObject go)
    {
        HideQuest();
    }
    void HideQuest()
    {
        questTween.PlayReverse();
        questTween.gameObject.SetActive(false);
    }

    public void OnSkillWolf()
    {
        if (isInTask)
            killCount++;

    }
    void onClickButton(GameObject go)
    {
        Button_Tpe index = Button_Tpe.BTN_ACCEPT;
        for(int i=0;i < allButtons.Length;i++)
        {
            if(go == allButtons[i])
            {
                index = ((Button_Tpe)i);

                break;
            }
        }

        switch(index)
        {
            case Button_Tpe.BTN_ACCEPT:
                showProgress();
                isInTask = true;
                break;
            case Button_Tpe.BTN_CANCEL:
                HideQuest();
                break;
            case Button_Tpe.BTN_OK:
                if(killCount >= 10)
                {
                    status.GetCoin(1000);
                    killCount = 0;
                    showTask();
                }
                else
                {
                    HideQuest();
                }
                break;
        }
    }
}
