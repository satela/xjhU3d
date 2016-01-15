using UnityEngine;
using System.Collections;

public class UrlManager {

	
    public static string GetEffectUrl(string names,EEffectType type)
    {
        string assetpath = "Assets";

        if (type == EEffectType.Attack)
        {
            return assetpath + "/RPG/Effect/effects/attackEffect/" + names + ".prefab";
        }
        else if (type == EEffectType.Beaton)
        {
            return assetpath + "/RPG/Effect/effects/beatenEffect/" + names + ".prefab";
        }
        else if (type == EEffectType.Move)
        {
            return assetpath + "/RPG/Effect/effects/attackEffect/" + names + ".prefab";
        }
        else if (type == EEffectType.Explode)
        {
            return assetpath + "/RPG/Effect/effects/attackEffect/" + names + ".prefab";
        }

        return "";
    }
}
