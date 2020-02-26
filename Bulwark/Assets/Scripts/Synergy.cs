using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    [CreateAssetMenu(menuName = "Bulwark/Synergy", order = 0)]
    public class Synergy : ScriptableObject {
        public new string name = "Synergy name";
        public List<SynergyTier> tiers;
    }

    public enum SynergyTargetType { SynergyTowersOnly, AllTowers };

    [System.Serializable]
    public class SynergyTier {
        [Tooltip("Unique towers necessary to activate this tier.")]
        public int towerCount = 2;
        [TextArea]
        [Tooltip("As would be written in a design document.")]
        public string description = "Description";
        [Tooltip("Should synergy be applied to only synergy towers or every tower on the field?")]
        public SynergyTargetType synergyTargetType = SynergyTargetType.AllTowers;
        [Tooltip("Duration to apply effect. (<= 0f means)")]
        public float duration = -1f;
        [Tooltip("Can the synergy be applied more than once per round?")]
        public bool repeating = true;
        [Tooltip("When to apply effect.")]
        public EffectTimeApplyOptions effectTimeApplyMode = EffectTimeApplyOptions.StartOfRound;
        [Tooltip("Effects applied natively by tower")]
        public List<TowerEffect> towerEffects;
        [Tooltip("Effects applied upon hitting unit")]
        public List<BulletEffect> bulletEffects;

    }
    
    public enum EffectValueApplyOptions { Set, Add, Multiply };
    public enum EffectTimeApplyOptions {  StartOfRound, AfterSeconds, AfterShotsFired, AfterUnitsKilled, AfterDamageTaken };
    public enum EffectUnitOptions { AllUnits, StandardUnits, FlyingUnits, BossUnits,
    NonBossUnits, NonStandardUnits, NonFlyingUnits, SlowedUnits, FrozenUnits,
    StunnedUnits, RootedUnits, BurningUnits, SunderedUnits, ShatteredUnits,
    SinnedUnits};

    [System.Serializable]
    public class TowerEffect {
        [Tooltip("Selected attribute to modify.")]
        public Tower.TowerPropertyOption property = Tower.TowerPropertyOption.AttackSpeed;
        [Tooltip("How to modify selected attribute.")]
        public EffectValueApplyOptions effectValueApplyMode = EffectValueApplyOptions.Add;
        [Tooltip("Value to be applied to selected attribute.")]
        public float value;
        
    }

    [System.Serializable]
    public class BulletEffect {
        public EffectValueApplyOptions effectType = EffectValueApplyOptions.Add;
        public Tower.TowerPropertyOption property = Tower.TowerPropertyOption.SlowPercent;
        public float value;
        public EffectUnitOptions effectUnitOptions = EffectUnitOptions.AllUnits;
    }
}
