using UnityEngine;
using System.Collections;

public class SkillItemIcon : UIDragDropItem
{

    private SkillInfo skillinfo;
	protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        skillinfo = transform.parent.GetComponent<SkillItemUI>().skillinfo;
        transform.parent = transform.root;
        this.GetComponent<UISprite>().depth = 100;
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if(surface != null && surface.tag == Tags.ShortCutSkill)
        {
            ShortCutSkillUI._instance.SetShortCutSkill(surface, skillinfo);
        }
    }
}
