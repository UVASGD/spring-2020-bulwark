using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDCGG {
    [CreateAssetMenu(menuName = "Bulwark/Synergy", order = 0)]
    public class Synergy : ScriptableObject {
        public new string name = "Synergy name";
        public List<SynergyTier> tiers;
    }

    public enum SynergyTargetType { synergyTowersOnly, allTowers };

    [System.Serializable]
    public class SynergyTier {
        public int towerCount = 2;
        [TextArea]
        public string description = "Description";
        public SynergyTargetType synergyTargetType = SynergyTargetType.allTowers;
        public List<TowerEffect> towerEffects;
        public List<BulletEffect> bulletEffects;

    }

    public enum EffectTarget { Tower, Unit };
    public enum EffectApplyType { Set, Add, Multiply, Percentage, Time };
    public enum EffectUnitOptions { AllUnits, StandardUnits, FlyingUnits, BossUnits,
    NonBossUnits, NonStandardUnits, NonFlyingUnits, SlowedUnits, FrozenUnits,
    StunnedUnits, RootedUnits, BurningUnits, SunderedUnits, ShatteredUnits,
    SinnedUnits};

    [System.Serializable]
    public class TowerEffect {
        public EffectApplyType effectType = EffectApplyType.Add;
        public float value;
        public Tower.TowerPropertyOption property = Tower.TowerPropertyOption.AttackSpeed;
    }

    [System.Serializable]
    public class BulletEffect {
        public EffectApplyType effectType = EffectApplyType.Add;
        public Tower.TowerPropertyOption property = Tower.TowerPropertyOption.SlowPercent;
        public float value;
        public EffectUnitOptions effectUnitOptions = EffectUnitOptions.AllUnits;
    }
}
