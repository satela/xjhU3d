using UnityEngine;
using System.Collections;

public class StatusManager : MonoBehaviour {

    public static StatusManager _instance;
    private TweenPosition tween;
    private bool isShow = false;

    public UILabel attacklbl;
    public UILabel deflbl;
    public UILabel speedlbl;
    public UILabel remainPtlbl;
    public UILabel summarylbl;

    public GameObject attackaddbtn;
    public GameObject defaddbtn;

    public GameObject speedaddbtn;

    private PlayerStatus playerStatus;
	// Use this for initialization
	void Start () {

        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
        gameObject.SetActive(false);

        UIEventListener.Get(attackaddbtn).onClick += onClickAddAttack;
        UIEventListener.Get(defaddbtn).onClick += onClickAddDef;

        UIEventListener.Get(speedaddbtn).onClick += onClickAddspeed;
        UIManager._instance.openCall += OnShowPanel;
	}

    private void OnShowPanel(EWindowName closeWindName)
    {
        if (closeWindName == EWindowName.Status)
            return;
        Hide();
    }

	
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TransformState()
    {
        if (isShow == false)
            Show();
        else
            Hide();
    }
    void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
        tween.PlayForward();

        UpdateView();
    }
    void Hide()
    {
        isShow = false;
        tween.PlayReverse();
    }

    //更新显示
    void UpdateView()
    {
        attacklbl.text = playerStatus.attack + " + " + playerStatus.attack_plus;
        deflbl.text = playerStatus.def + " + " + playerStatus.def_plus;
        speedlbl.text = playerStatus.speed + " + " + playerStatus.speed_plus;

        remainPtlbl.text = playerStatus.point_remain.ToString();

        summarylbl.text = "伤害：" + (playerStatus.attack + playerStatus.attack_plus) +
                            " " + "防御：" + (playerStatus.def_plus + playerStatus.def) +
                            " " + "速度：" + (playerStatus.speed_plus + playerStatus.speed);
        if(playerStatus.point_remain > 0)
        {
            attackaddbtn.SetActive(true);
            defaddbtn.SetActive(true);
            speedaddbtn.SetActive(true);
        }
        else
        {
            attackaddbtn.SetActive(false);
            defaddbtn.SetActive(false);
            speedaddbtn.SetActive(false);
        }

    }

    void onClickAddAttack(GameObject go)
    {
        playerStatus.DistributeRemainPoint(ERoleAttType.ATTACK);
        UpdateView();
    }

    void onClickAddDef(GameObject go)
    {
        playerStatus.DistributeRemainPoint(ERoleAttType.DEFENSE);
        UpdateView();

    }

    void onClickAddspeed(GameObject go)
    {
        playerStatus.DistributeRemainPoint(ERoleAttType.SPEED);
        UpdateView();

    }

}
