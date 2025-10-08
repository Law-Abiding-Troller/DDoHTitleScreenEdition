using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace HoverfishTitleScreen;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static ConfigOptions Options;

    public static GameObject HoverFishPrefab;

    public static IEnumerator GetHoverFishPrefab(BaseUnityPlugin plugin)
    {
        CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.Hoverfish);
        yield return task;
        HoverFishPrefab = task.GetResult();
        if (HoverFishPrefab == null) yield break;
        var wf = HoverFishPrefab.GetComponent<WorldForces>();
        float aWG = 0.5f;
        if (wf == null) PrefabUtils.AddWorldForces(HoverFishPrefab, 5).aboveWaterGravity = aWG;
        else wf.aboveWaterGravity = aWG;
        RemoveComponents();
        var deez = Instantiate(new GameObject("deez"));
        deez.AddComponent<UpdateScheduler>().updateFrequency = 1;
        TitleScreen.Register(plugin);
    }

    private void Awake()
    {
        Options = OptionsPanelHandler.RegisterModOptions<ConfigOptions>();
        // set project-scoped logger instance
        Logger = base.Logger;
        StartCoroutine(GetHoverFishPrefab(this));
        // Initialize custom prefabs
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        WaitScreenHandler.RegisterEarlyLoadTask("DDoH",AddComponents);
        SaveUtils.RegisterOnQuitEvent(RemoveComponents);
    }

    public static void RemoveComponents()
    {
        var lwe = HoverFishPrefab.GetComponent<LargeWorldEntity>();
        if (lwe != null){ Destroy(lwe); }
    }

    public static void AddComponents(WaitScreenHandler.WaitScreenTask task)
    {
        task.Status = ("DDOH is repairing damage done to your game by DDOH");
        HoverFishPrefab.EnsureComponent<LargeWorldEntity>();
    }
}