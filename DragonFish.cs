using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;

namespace HoverfishTitleScreen;

public class DragonFish : CreatureAsset
{
    public DragonFish(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.Bundle.LoadAsset<GameObject>(PrefabInfo.ClassID),
            BehaviourType.MediumFish, EcoTargetType.MediumFish, 320f);
        CreatureTemplateUtils.SetCreatureDataEssentials(template, LargeWorldEntity.CellLevel.Medium, 100f);
        CreatureTemplateUtils.SetCreatureMotionEssentials(template,
            new SwimRandomData(0.4f, 4f, new Vector3(40f, 10f, 40f)), 
            new StayAtLeashData(0.8f,5f, 8f, 28f));
        template.SetCreatureComponentType<DragonFishComponent>();
        template.AvoidObstaclesData = new AvoidObstaclesData(1f, 4f, false, 5f, 10f);
        template.SizeDistribution = new AnimationCurve();
        template.AnimateByVelocityData = new AnimateByVelocityData(8f);
        template.PickupableFishData = new PickupableFishData()
        {
            CanBeHeld = false,
            WorldModelName = "DragonFish",
            ViewModelName = "DragonFish"
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.1f, 0.5f, 1f, 1.5f, true, false, ClassID));
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var tM = new TrailManagerBuilder(components, prefab.transform.SearchChild("Spine.001"), 2.5f, 1f);
        tM.SetTrailArrayToChildrenWithCondition(ts =>
        {
            if (ts.name.Contains("Spine")) return true;
            return false;
        });
        tM.Apply();
        yield return null;
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        base.ApplyMaterials(prefab);
        if (!prefab.TryGetComponent<SkinnedMeshRenderer>(out var sMr)) return;
        sMr.material.SetFloat(ShaderPropertyID._MyCullVariable, 0f);
    }
}

internal class DragonFishComponent : Creature
{
}