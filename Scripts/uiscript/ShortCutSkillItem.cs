using UnityEngine;
using System.Collections;

public class ShortCutSkillItem : UIDragDropItem{

    private UISprite sprite;
    public SkillInfo skillinfo;

    public ObjectInfo iteminfo;
    private PlayerAttack plattack;

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
    }

	// Use this for initialization
	void Start () {

        plattack = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAttack>();
        UIEventListener.Get(gameObject).onClick += OnClickUseSkill;
	}

    private void OnClickUseSkill(GameObject go)
    {
        if (skillinfo != null)
        {
            if ((int)skillinfo.applicableRole == (int)PlayerStatus._instance.heroType)
            {
                if (PlayerStatus._instance.useMp(skillinfo.mp))
                {
                    plattack.useSkill(skillinfo);
                }
            }
        }

        else if(iteminfo != null) 
        {
            if (PlayerStatus._instance.useItems(iteminfo.id, 1))
            {
                PlayerStatus._instance.useDrugEff(iteminfo.hp, iteminfo.mp);

            }          

        }


    }

   

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        ShortCutSkillUI._instance.clearSkillOrItem(transform.parent.gameObject);
        transform.parent = transform.root;
        sprite.depth = 100;
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if (surface != null)
        {
            if (surface.tag == Tags.ShortCutSkill)
            {
                if (skillinfo != null)
                    ShortCutSkillUI._instance.SetShortCutSkill(surface, skillinfo);
                else if(iteminfo != null)
                    ShortCutSkillUI._instance.SetShortCutItem(surface, iteminfo);
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
