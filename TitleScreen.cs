using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using Nautilus.Handlers.TitleScreen;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace HoverfishTitleScreen;

public class TitleScreen
{
    public static WorldTitleObjectHandler HoverfishWTOBH = new(SpawnNothing);
    public static void Register(BaseUnityPlugin plugin)
    {
        TitleScreenHandler.RegisterTitleScreenObject("DDOH",new TitleScreenHandler.CustomTitleData(
            "DDoH, Title Edition", HoverfishWTOBH));
    }

    public static GameObject SpawnNothing()
    {
        var _subnauticaLogo = GameObject.Find("logo");
        GameObject NRE = new GameObject("Hoverfish Parent");
        if (_subnauticaLogo == null) return new GameObject("NRE");
        var newVector3 = new Vector3( _subnauticaLogo.transform.position.x,_subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z+30);
        GameObject hoverfishObject = Object.Instantiate(Plugin.HoverFishPrefab, newVector3, _subnauticaLogo.transform.rotation, NRE.transform);
        return NRE;
    }
}

public class WorldTitleObjectHandler : WorldObjectTitleAddon
{
    private int _up = 0;
    private GameObject _subnauticaLogo;
    private GameObject _hoverishObject;
    private Vector3 _targetPosition;
    private Vector3 _targetScale;
    public static bool Enabled;
    private List<GameObject> hoverfishesObj = new();
    private List<Renderer> _renderers = new List<Renderer>();
    private List<Graphic> _graphics = new List<Graphic>();
    private bool _fadingIn;
    private float _currentFadeInTime;
    public WorldTitleObjectHandler(Func<GameObject> spawnObject,  float fadeInTime = 1, params string[] requiredGUIDs) : base(spawnObject, fadeInTime, requiredGUIDs)
    {
    }

    protected override void OnInitialize()
    {
        _subnauticaLogo = GameObject.Find("logo");
        if (_subnauticaLogo == null) return;
        var newVector3 = new Vector3( _subnauticaLogo.transform.position.x,_subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
        Vector3[] newVector33 =
        {
            new (newVector3.x,newVector3.y+1,newVector3.z-20),
            new (newVector3.x+5,newVector3.y+1,newVector3.z-20),
            new Vector3(newVector3.x+10,newVector3.y+1,newVector3.z-20), 
            new Vector3(newVector3.x+15,newVector3.y+1,newVector3.z-20),
            new Vector3(newVector3.x+20,newVector3.y+1,newVector3.z-20), 
            new Vector3(newVector3.x+25,newVector3.y+1,newVector3.z-20),
            new Vector3(newVector3.x+30,newVector3.y+1,newVector3.z-20),
            new Vector3(newVector3.x+35,newVector3.y+1,newVector3.z-20),
            new Vector3(newVector3.x+40,newVector3.y+1,newVector3.z-20),
            new Vector3(newVector3.x,newVector3.y+1,newVector3.z-15),
            new Vector3(newVector3.x+5,newVector3.y+1,newVector3.z-15),
            new Vector3(newVector3.x+10,newVector3.y+1,newVector3.z-15), 
            new Vector3(newVector3.x+15,newVector3.y+1,newVector3.z-15),
            new Vector3(newVector3.x+20,newVector3.y+1,newVector3.z-15), 
            new Vector3(newVector3.x+25,newVector3.y+1,newVector3.z-15),
            new Vector3(newVector3.x+30,newVector3.y+1,newVector3.z-15),
            new Vector3(newVector3.x+35,newVector3.y+1,newVector3.z-15),
            new Vector3(newVector3.x+40,newVector3.y+1,newVector3.z-15),
            new Vector3(newVector3.x,newVector3.y+1,newVector3.z-25),
            new Vector3(newVector3.x+5,newVector3.y+1,newVector3.z-25),
            new Vector3(newVector3.x+10,newVector3.y+1,newVector3.z-25), 
            new Vector3(newVector3.x+15,newVector3.y+1,newVector3.z-25),
            new Vector3(newVector3.x+20,newVector3.y+1,newVector3.z-25), 
            new Vector3(newVector3.x+25,newVector3.y+1,newVector3.z-25),
            new Vector3(newVector3.x+30,newVector3.y+1,newVector3.z-25),
            new Vector3(newVector3.x+35,newVector3.y+1,newVector3.z-25),
            new Vector3(newVector3.x+40,newVector3.y+1,newVector3.z-25),
        };
        int j = 0;
        for (int i = 0; i < Plugin.Options.HoverFishCount*Plugin.Options.Multiplier; i++)
        {
            if (j == newVector33.Length) j = 0;
            hoverfishesObj.Add(Object.Instantiate(Plugin.HoverFishPrefab, newVector33[j],_subnauticaLogo.transform.rotation, WorldObject.transform));
            hoverfishesObj[i].SetActive(true);
            foreach (var r in hoverfishesObj[i].GetComponentsInChildren<Renderer>(true)) if (r != null) _renderers.Add(r);
            foreach (var g in hoverfishesObj[i].GetComponentsInChildren<Graphic>(true)) if (g != null) _graphics.Add(g);
            j++;
        }
        //_targetPosition = new Vector3(MainCamera._camera.transform.position.x, MainCamera._camera.transform.position.y, MainCamera._camera.transform.position.z);
        _targetScale = Vector3.one*2;
        if (WorldObject.name == "NRE") return;
        _hoverishObject = hoverfishesObj[0];
        base.OnInitialize();
    }
    private void UpdateObjectOpacities(float alpha)
    {
        if (!WorldObject) return;

        foreach (var rend in _renderers)
        {
            rend.SetFadeAmount(alpha);
        }
        
        foreach (var graphic in _graphics)
        {
            var col = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            graphic.color = col;
        }
    }

    public void MoreHoverfish(int num)
    {
        for (int i = 0; i < num; i++) hoverfishesObj.Add(Object.Instantiate(Plugin.HoverFishPrefab, hoverfishesObj[i].transform.position, Quaternion.identity, WorldObject.transform));
    }

    public void LessHoverfish()
    {
        
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Enabled = true;
        _up = 1;
        foreach (var obj in hoverfishesObj)
        {
            obj.SetActive(true);
        }
        BehaviourUpdateUtils.Register(this);
        _currentFadeInTime = 0;
        _fadingIn = true;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Enabled = false;
        _up = 0;
        foreach (var obj in hoverfishesObj)
        {
            obj.SetActive(false);
        }
        if (!_fadingIn)
            _currentFadeInTime = 0;
        else
            _currentFadeInTime = FadeInTime - _currentFadeInTime;
        _fadingIn = false;
    }

    public override void ManagedUpdate()
    {
        base.ManagedUpdate();
        if (_hoverishObject == null) return;
        if (_up != 0)
        {
            _hoverishObject.transform.position = Vector3.MoveTowards(_hoverishObject.transform.position, _targetPosition, Time.deltaTime*5);
            _hoverishObject.transform.localScale = Vector3.MoveTowards(_hoverishObject.transform.localScale, _targetScale, Time.deltaTime*2);
        }

        if (_hoverishObject.transform.position == _targetPosition )
        {
            switch (_up)
            {
                case 1:
                    _up = 2;
                    _targetPosition = new Vector3(_subnauticaLogo.transform.position.x + 30f,
                        _subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
                    _targetScale = Vector3.one*2;
                    break;
                case 2:
                    _up = 3;
                    _targetPosition = new Vector3(MainCamera._camera.transform.position.x,
                        MainCamera._camera.transform.position.y, MainCamera._camera.transform.position.z+20f);
                    _targetScale = Vector3.one;
                    break;
                case 3:
                    _up = 4;
                    _targetPosition = new Vector3( _subnauticaLogo.transform.position.x - 30f,
                        _subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
                    _targetScale = Vector3.one*2;
                    break;
                case 4:
                    _up = 1;
                    _targetPosition = new Vector3(MainCamera._camera.transform.position.x, 
                        MainCamera._camera.transform.position.y, MainCamera._camera.transform.position.z+20f);
                    _targetScale = Vector3.one;
                    break;
                default:
                    _up = 0;
                    break;
            }
        }
        if (!WorldObject)
        {
            BehaviourUpdateUtils.Deregister(this);
            return;
        }
        
        if (_currentFadeInTime < FadeInTime)
        {
            _currentFadeInTime += Time.deltaTime;
            float normalizedProgress = _currentFadeInTime / Mathf.Max(FadeInTime, float.Epsilon);
            UpdateObjectOpacities(_fadingIn ? normalizedProgress : 1 - normalizedProgress);

            if (!_fadingIn && normalizedProgress >= 1)
            {
                WorldObject.SetActive(false);
            }
            else if (_fadingIn && normalizedProgress > 0)
            {
                WorldObject.SetActive(true);
            }
        }
        else if (!_fadingIn)
        {
            BehaviourUpdateUtils.Deregister(this);
        }
    }
}

[HarmonyPatch(typeof(BehaviourLOD))]
public class BehaviourLODPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(BehaviourLOD.Update))]
    public static bool Update_Prefix(BehaviourLOD __instance)
    {
        if (!WorldTitleObjectHandler.Enabled) return true;
        __instance.current = LODState.Full;
        return false;
    }
}