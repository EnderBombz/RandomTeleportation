using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace RandomSpawn
{
    [BepInPlugin("EnderBombz.RandomSpawn", "RandomSpawn", "1.0.0")]
    [BepInProcess("valheim.exe")]

    public class RandomSpawn : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("EnderBombz.RandomSpawn");

        void Awake()
        {
            Harmony harmony = new Harmony("mod.randomspawn");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Player), "OnSpawned")] /*aqui é onde o vai verificar a ação do usuário e disparar o plugin*/

        class OnSpawned_patch
        {
            static void Postfix(ref Player __instance)
            {

                Debug.Log("iniciando debug...");
          
                System.Random rnd = new System.Random();
                int x = rnd.Next(-10000, 10000);  // creates a number between 1 and 12
                int z = rnd.Next(-10000, 10000);
                Vector3 val = new Vector3(x, 5.0f, z);

                ZoneSystem zone = new ZoneSystem();
                //ZoneSystem.instance.SpawnLocation();
                
               
                    Debug.Log($"Teleportaded to ({x},{z})");
                    Debug.Log($"This biome is: {__instance.GetCurrentBiome()}");
                    __instance.transform.position = new Vector3(x, 5.0f, z);
                    Debug.Log("finalizando debug.");
            }
        }
        [HarmonyPatch(typeof(Player), "FixedUpdate")] /*aqui é onde o vai verificar a ação do usuário e disparar o plugin*/
        class FixedUpdate_patch
        {
          static void PostFix(Player __instance){
                Debug.Log($"This biome is: {__instance.GetCurrentBiome()}");
            }
        }
    }
}