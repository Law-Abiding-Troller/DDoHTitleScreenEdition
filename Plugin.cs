using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.Utils;
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

    public static GameObject UpdateScheduler;

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
        Logger.LogDebug("Pre component Removal");
        /*foreach (var component in HoverFishPrefab.GetAllComponentsInChildren<Component>())
        {
            Logger.LogDebug(component.GetType());
        }*/
        RemoveComponents();
        /*Logger.LogDebug("Post component Removal");
        foreach (var component in HoverFishPrefab.GetAllComponentsInChildren<Component>())
        {
            Logger.LogDebug(component.GetType());
        }*/
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
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
        WaitScreenHandler.RegisterEarlyLoadTask("DDoH Title Edition",AddComponents);
        SaveUtils.RegisterOnQuitEvent(RemoveComponents);
    }

    public static void RemoveComponents()
    {
        var lwe = HoverFishPrefab.GetComponent<LargeWorldEntity>();
        if (lwe != null){ Destroy(lwe); }
        var scared = HoverFishPrefab.GetComponent<Scareable>();
        if (scared != null) { Destroy(scared); }
        var fws = HoverFishPrefab.GetComponent<FleeWhenScared>();
        if (fws != null) { Destroy(fws); }
        var im = HoverFishPrefab.GetComponent<InfectedMixin>();
        if (im != null) { Destroy(im); }
        UpdateScheduler = Instantiate(new GameObject("deez"));
        var cf = HoverFishPrefab.GetComponent<CreatureFear>();
        if (cf != null) { Destroy(cf); }
        var sthp = HoverFishPrefab.GetComponent<SwimToHeroPeeper>();
        if (sthp != null) { Destroy(sthp); }
        var us = UpdateScheduler.AddComponent<UpdateScheduler>();
        us.updateFrequency = 1;
        us.updateTimer = Time.deltaTime;
    }

    public static void AddComponents(WaitScreenHandler.WaitScreenTask task)
    {
        task.Status = ("DDOH is repairing damage done to your game by DDOH");
        HoverFishPrefab.EnsureComponent<LargeWorldEntity>();
        HoverFishPrefab.EnsureComponent<Scareable>();
        HoverFishPrefab.EnsureComponent<FleeWhenScared>();
        HoverFishPrefab.EnsureComponent<InfectedMixin>();
        HoverFishPrefab.EnsureComponent<CreatureFear>();
        HoverFishPrefab.EnsureComponent<SwimToHeroPeeper>();
        if (UpdateScheduler != null) Destroy(UpdateScheduler);
    }
}