using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveEffect : MonoBehaviour {

    private DBaseFightRole tragetfightrole;

    private SkillCasterData skillcastdata;

    private float speeds = 0.2f;

    private bool startFollow = false;
   // private EffectAsset effectasset;
	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {

        if (startFollow && tragetfightrole != null)
        {
            Vector3 diffvew = tragetfightrole.rolePosition + Vector3.up - transform.position;
            if (diffvew.magnitude < 0.1)
            {
                startFollow = false;
                showHitMovie();
            }

            else
            {
                transform.position = transform.position +  diffvew.normalized * speeds;
            }

        }
	}

    public void SetTargetPos(Vector3 targetpos, SkillCasterData skilldata)
    {
        skillcastdata = skilldata;
        iTween.MoveTo(gameObject, iTween.Hash("position", targetpos, "easeType", "easeInOutExpo", "delay", .1, "time",0.5f,"oncomplete","destroyeffect"));
    }

    public void SetTargetPos(GameObject targetRole, SkillCasterData skilldata)
    {
        skillcastdata = skilldata;
        tragetfightrole = targetRole.GetComponent<DBaseFightRole>();
        if (tragetfightrole != null)
        startFollow = true;
        //DBaseFightRole role = targetRole.GetComponent<DBaseFightRole>();
      
       // iTween.MoveTo(gameObject, iTween.Hash("position", role.roleModel.transform.position + Vector3.up, "easeType", "easeInOutExpo",  "time",0.7f,"delay", .1, "oncomplete", "showHitMovie"));
    }

    
    void showHitMovie()
    {
        if (skillcastdata != null && skillcastdata.skilldata != null)
        {
            if (!string.IsNullOrEmpty(skillcastdata.skilldata.explodeEffUrl))
            {
                GameObject explodeEff = new GameObject(skillcastdata.skilldata.explodeEffUrl);
                EffectAsset effectasset = explodeEff.AddComponent<EffectAsset>();

                effectasset.setEffectParam(skillcastdata.skilldata.explodeEffUrl, transform.position, Quaternion.identity, EEffectType.Explode);
            }

        }
        if (tragetfightrole != null)
        {
            DBaseFightRole fightrole = tragetfightrole.GetComponent<DBaseFightRole>();
            if (fightrole != null)
            {
                fightrole.showBeatonBySkill(skillcastdata);

            }
        }
        Destroy(gameObject);
    }
    void destroyeffect()
    {      
        if (skillcastdata != null && skillcastdata.skilldata != null)
        {
            if (!string.IsNullOrEmpty(skillcastdata.skilldata.explodeEffUrl))
            {
                GameObject explodeEff = new GameObject(skillcastdata.skilldata.explodeEffUrl);
                EffectAsset effectasset = explodeEff.AddComponent<EffectAsset>();

                effectasset.setEffectParam(skillcastdata.skilldata.explodeEffUrl, transform.position, Quaternion.identity, EEffectType.Explode);
            }

            if (skillcastdata.skilldata.isQunGong == false)
            {

            }
            else
            {
                List<GameObject> beatonRoles = FightRoleManager._instance.getHarmListByDist(skillcastdata.castRole.GetComponent<DBaseFightRole>().side, transform.position, skillcastdata.skilldata.harmDist,skillcastdata.skilldata.isAttackSkill());

                DBaseFightRole fightole;
                foreach (GameObject roles in beatonRoles)
                {
                    fightole = roles.GetComponent<DBaseFightRole>();
                    if (fightole != null)
                    {
                        fightole.showBeatonBySkill(skillcastdata);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
