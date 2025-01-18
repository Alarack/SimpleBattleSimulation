using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.Events;

public static class StatAdjustmentManager {


  
 
    public static float ApplyStatAdjustment(Entity target, StatModifierData modData, Entity source, float multiplier = 1f) {


        Action<StatName, float, StatModType, object> statModAction = modData.variantTarget switch {
            StatModifierData.StatVariantTarget.Simple => target.Stats.AddModifier,
            StatModifierData.StatVariantTarget.RangeCurrent => target.Stats.AdjustStatRangeCurrentValue,
            StatModifierData.StatVariantTarget.RangeMin => target.Stats.AddMinValueModifier,
            StatModifierData.StatVariantTarget.RangeMax => target.Stats.AddMaxValueModifier,
            _ => null,
        };


        float modValue = modData.value * multiplier;

        statModAction?.Invoke(modData.targetStat, modValue, modData.modifierType, source);

        SendStatChangeEvent(modData.targetStat, target, source, modValue);

        return modValue;
    }


    public static float ApplyStatAdjustment(Entity target, StatModifier mod, StatModifierData.StatVariantTarget variant, Entity source, float multiplier = 1f) {
        Action<StatName, StatModifier> statModAction = variant switch {
            StatModifierData.StatVariantTarget.Simple => target.Stats.AddModifier,
            StatModifierData.StatVariantTarget.RangeCurrent => target.Stats.AdjustStatRangeCurrentValue,
            StatModifierData.StatVariantTarget.RangeMin => target.Stats.AddMinValueModifier,
            StatModifierData.StatVariantTarget.RangeMax => target.Stats.AddMaxValueModifier,
            _ => null,
        };


        float modValue = mod.Value * multiplier;

        mod.UpdateModValue(modValue);

        statModAction?.Invoke(mod.TargetStat, mod);

        SendStatChangeEvent(mod.TargetStat, target, source, mod.Value);

        return modValue;
    }

    public static float RemoveStatAdjustment(Entity target, StatModifier mod, StatModifierData.StatVariantTarget variant, Entity source) {

        if (target.Stats.Contains(mod.TargetStat) == false) {
            //Debug.LogWarning(target.EntityName + " does not have " + mod.TargetStat + " whem removing.");
            return 0f;
        }

        Action<StatName, StatModifier> statModAction = variant switch {
            StatModifierData.StatVariantTarget.Simple => target.Stats.RemoveModifier,
            StatModifierData.StatVariantTarget.RangeCurrent => null, /*when removeRangeAdjsument == true => target.Stats.RemoveCurrentRangeAdjustment*/
            StatModifierData.StatVariantTarget.RangeMin => target.Stats.RemoveMinValueModifier,
            StatModifierData.StatVariantTarget.RangeMax => target.Stats.RemoveMaxValueModifier,
            _ => null,
        };

        statModAction?.Invoke(mod.TargetStat, mod);

        SendStatChangeEvent(mod.TargetStat, target, source, mod.Value, true);

        return mod.Value;
    }

    public static float ApplyStatAdjustment(Entity target, float value, StatName targetStat, StatModType modType, StatModifierData.StatVariantTarget statVariant, object source, float multiplier = 1f, bool addMissingStat = false, Entity delivery = null) {
        StatModifier mod = new StatModifier(value, modType, targetStat, source, statVariant);
        return ApplyStatAdjustment(target, mod, targetStat, statVariant, multiplier, addMissingStat, delivery);
    }

    public static float ApplyStatAdjustment(Entity target, StatModifier mod, StatName targetStat, StatModifierData.StatVariantTarget statVarient, float multiplier = 1f, bool addMissingStat = false, Entity delivery = null) {

        if(target.IsInvincible == true && targetStat == StatName.Health && mod.Value < 0f && statVarient == StatModifierData.StatVariantTarget.RangeCurrent) {
            return 0f;
        }

        if (target.Stats.Contains(mod.TargetStat) == false) {

            if (addMissingStat == false) {
                return 0f;
            }
            SimpleStat newStat = new SimpleStat(targetStat, 0f);
            target.Stats.AddStat(newStat);
        }

        Action<StatName, StatModifier> statModAction = statVarient switch {
            StatModifierData.StatVariantTarget.Simple => target.Stats.AddModifier,
            StatModifierData.StatVariantTarget.RangeCurrent => target.Stats.AdjustStatRangeCurrentValue,
            StatModifierData.StatVariantTarget.RangeMin => target.Stats.AddMinValueModifier,
            StatModifierData.StatVariantTarget.RangeMax => target.Stats.AddMaxValueModifier,
            _ => null,
        };

        mod.UpdateModValue(mod.Value * multiplier);

        statModAction?.Invoke(targetStat, mod);

        try {
            SendStatChangeEvent(targetStat, target, (Entity)mod.Source, mod.Value, false, delivery);
        }
        catch (System.Exception e) {
            Debug.LogError(e.Message);
            Debug.LogError("We're assuming all mod sources are Entities, but one is being sent in that isnt an entity");
            Debug.LogError("Souce: " + mod.Source.GetType().Name);
        }

        return mod.Value;

    }

    private static void SendStatChangeEvent(StatName targetStat, Entity target, Entity source, float changeValue, bool isRemoveal = false, Entity delivery = null) {
        EventData eventData = new EventData();
        eventData.AddEntity("Target", target);
        eventData.AddEntity("Source", source);
        eventData.AddEntity("Delivery", delivery);
        eventData.AddFloat("Value", changeValue);
        eventData.AddInt("Stat", (int)targetStat);
        eventData.AddBool("Removal", isRemoveal);

        EventManager.SendEvent(GameEvent.UnitStatAdjusted, eventData);
    }
}
