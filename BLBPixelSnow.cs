using UnityEngine;
using System.Collections.Generic;
using DaggerfallConnect;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;   //required for modding features
using DaggerfallWorkshop;

public class BLBPixelSnow : MonoBehaviour
{
    public static Mod Mod {
        get;
        private set;
    }

    public static Material SnowMaterial;
    public static float minParticleSize = 0.002f;
    public static float maxParticleSize = 0.0025f;
    public static int maxParticles = 17500;

    public static BLBPixelSnow Instance { get; private set; }

    [Invoke(StateManager.StateTypes.Start, 0)]
    public static void Init(InitParams initParams)
    {
        Mod = initParams.Mod;  // Get mod
        var settings = Mod.GetSettings();

        int minPS = settings.GetValue<int>("SnowSize","minParticleSize");
        int maxPS = settings.GetValue<int>("SnowSize","maxParticleSize");
        int maxP = settings.GetValue<int>("MaxParticles", "MaxParticles");
        

        minParticleSize = (minPS / 100000f);
        maxParticleSize = (maxPS / 100000f);
        maxParticles = (int) (maxP * 1000);

        Debug.Log("Min particle size: " + minParticleSize.ToString());
        Debug.Log("Max particle size: " + maxParticleSize.ToString());
        Debug.Log("Max particles: " + maxParticles.ToString());

        Instance = new GameObject("BLBPixelSnow").AddComponent<BLBPixelSnow>(); // Add script to the scene.
        SnowMaterial = Mod.GetAsset<Material>("Materials/BLBSnowMaterial") as Material;
        GameObject playerAdvanced = GameObject.Find("PlayerAdvanced");
        if(playerAdvanced != null) {
            //Debug.Log("BLB: PlayerAdvanced found");
            GameObject smoothFollower = playerAdvanced.transform.Find("SmoothFollower").gameObject;
            if(smoothFollower != null) {
                //Debug.Log("BLB: SmoothFollower found");
                GameObject snowParticles = smoothFollower.transform.Find("Snow_Particles").gameObject;
                if(snowParticles != null) {
                    //Debug.Log("BLB: SnowParticles found");
                    ParticleSystem ps = snowParticles.GetComponent<ParticleSystem>();
                    if(ps != null) {
                        //Debug.Log("BLB: Particle system found");
                        ParticleSystem.MainModule main = ps.main;
                        main.startRotation = 0;

                        ParticleSystem.RotationOverLifetimeModule psROL = ps.rotationOverLifetime;
                        psROL.enabled = false;

                        ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
                        psr.material = SnowMaterial;
                        psr.minParticleSize = minParticleSize;
                        psr.maxParticleSize = maxParticleSize;
                    }
                }
            }
        }
        Debug.Log("blb-pixel-snow initialized");
    }

    void Awake ()
    {
        Mod.IsReady = true;
        Debug.Log("blb-pixel-snow awakened");
    }    
}