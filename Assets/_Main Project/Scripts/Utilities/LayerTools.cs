using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MaskTargeting {
    Opposite,
    Same,
}


public static class LayerTools {


    public static bool IsLayerInMask(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static LayerMask AddToMask(LayerMask mask, int layer) {
        //Debug.Log("Adding: " + LayerMask.LayerToName(layer));
        
       return mask |= (1 << layer);
    }

    public static LayerMask RemoveFromMask(LayerMask mask, int layer) {
        return mask &= ~(1 << layer);
    }

    public static LayerMask SetupHitMask(LayerMask mask, int sourceLayer, MaskTargeting targeting = MaskTargeting.Opposite) {

        mask = LayerMask.LayerToName(sourceLayer) switch {
            "Enemy" when targeting == MaskTargeting.Opposite => AddToMask(mask, LayerMask.NameToLayer("Player")),
            "Enemy" when targeting == MaskTargeting.Same => AddToMask(mask, LayerMask.NameToLayer("Enemy")),
            "Player" when targeting == MaskTargeting.Opposite => AddToMask(mask, LayerMask.NameToLayer("Enemy")),
            "Player" when targeting == MaskTargeting.Same => AddToMask(mask, LayerMask.NameToLayer("Player")),
            _ => mask,
        };

        return mask;

    }


}
