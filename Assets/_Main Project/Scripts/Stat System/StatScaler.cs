using UnityEngine;


public class StatScaler {

    public StatName targetStat;
    public StatModifierData.DeriveFromWhom deriveTarget;
    public float statScaleBaseValue;
    public SimpleStat scalerStat;

    public StatScaler() {

    }

    public void InitStat() {
        scalerStat = new SimpleStat(StatName.StatScaler, statScaleBaseValue);
    }

    public StatScaler(StatScaler clone) {
        //this.type = clone.type;
        this.targetStat = clone.targetStat;
        this.statScaleBaseValue = clone.statScaleBaseValue;
        this.deriveTarget = clone.deriveTarget;


        if (clone.scalerStat == null) {
            this.scalerStat = new SimpleStat(StatName.StatScaler, clone.statScaleBaseValue);
        }
        else {
            this.scalerStat = new SimpleStat(clone.scalerStat);
        }
    }

    public void AddScalerMod(StatModifier mod) {
        scalerStat.AddModifier(mod);
    }

    public void RemoveScalerMod(StatModifier mod) {
        scalerStat.RemoveModifier(mod);
    }

    public void RemoveAllScalarModsFromSource(object source) {
        scalerStat.RemoveAllModifiersFromSource(source);
    }
}

