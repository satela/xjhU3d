using UnityEngine;
using System.Collections;

public class ShopDrug : MonoBehaviour {

    public static ShopDrug _instance;
    private TweenPosition tween;
    private bool isShow = false;
    public GameObject closebtn;

    public GameObject numberInputDlg;

    public UIInput numInput;

    public GameObject[] buyBtns = new GameObject[3];

    public GameObject okbtn;
    private int buyId = 0;
    public void Awake()
    {
        _instance = this;
    }


	// Use this for initialization
	void Start () {

        UIEventListener.Get(closebtn).onClick += OnClickClose;
        UIEventListener.Get(okbtn).onClick += OnClickbuyOk;

        tween = this.GetComponent<TweenPosition>();
        for (int i = 0; i < buyBtns.Length; i++)
            UIEventListener.Get(buyBtns[i]).onClick += OnClickbuyItem;

            gameObject.SetActive(false);
            UIManager._instance.openCall += OnShowPanel;
	}

    private void OnShowPanel(EWindowName closeWindName)
    {
        if (closeWindName == EWindowName.ShopDrug)
            return;
        Hide();
    }

    void OnClickClose(GameObject go)
    {
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

       // UpdateView();
    }
    void Hide()
    {
        isShow = false;
        tween.PlayReverse();
    }

    private void OnTweenFinished()
    {
        if (isShow == false)
            gameObject.SetActive(false);

    }
    void OnClickbuyItem(GameObject go)
    {
        numberInputDlg.SetActive(true);
        numInput.value = "0";
        if (go == buyBtns[0])
            buyId = 1001;
        else if (go == buyBtns[1])
            buyId = 1002;
        else
            buyId = 1003;          
    }

    void OnClickbuyOk(GameObject go)
    {
        OnBuyID();
        
    }
    public void OnBuyID()
    {
        int count = int.Parse(numInput.label.text);
        ObjectInfo info = ObjectsInfo._instance.GetInfoById(buyId);
        if(count > 0 && info != null)
        {
            int price = info.price_buy;
            int total = price * count;
            bool sucess = PlayerStatus._instance.getCoin(total);
            if(sucess)
            {
                PlayerStatus._instance.addItem(buyId, count);
            }
        }

        numberInputDlg.SetActive(false);
    }
}
