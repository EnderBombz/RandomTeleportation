using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RandomSpawn
{
    [BepInPlugin("EnderBombz.RandomSpawn", "RandomSpawn", "1.0.0")]
    [BepInProcess("valheim.exe")]

    public class RandomSpawn : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("EnderBombz.RandomSpawn");
        public static Vector3 worldSpawnSetted;
        public static bool dead = false;


        void Awake()
        {
            Harmony harmony = new Harmony("mod.randomspawn");
            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(Player), "OnDeath")]
        class OnDeath_patch
        {
            static void Prefix()
            {
                dead = true;
            }
            static void Postfix()
            {
                dead = true;
            }
        }


        [HarmonyPatch(typeof(Player), "OnSpawned")]

        class OnSpawned_patch
        {
            public static PlayerProfile profile = Game.instance.m_playerProfile;
            public static Vector3 worldSpawn = Game.instance.m_playerProfile.GetHomePoint();

            public static Bed verifyBedNearby(Vector3 customSpawn, float max)
            {
                Bed[] array = Object.FindObjectsOfType<Bed>();
                foreach (Bed bed in array)
                {
                    if (bed.IsCurrent())
                    {
                        return bed;
                    }
                }
                return null;
            }

            public static void Query(Player __instance)
            {
                worldSpawnSetted = worldSpawn;
                var stoneCircle = new List<Vector3>();
                var lastHomePoint = worldSpawn;
                if (__instance.m_firstSpawn)
                {
                    foreach (KeyValuePair<Vector2i, ZoneSystem.LocationInstance> keyValuePair in ZoneSystem.instance.m_locationInstances)
                    {
                        if (keyValuePair.Value.m_location.m_prefabName.ToLower().Contains("stonecircle"))
                        {
                            stoneCircle.Add(keyValuePair.Value.m_position);
                        }
                    }
                    System.Random random = new System.Random();
                    int index = random.Next(stoneCircle.Count);
                    __instance.transform.position = stoneCircle[index];
                    profile.SetHomePoint(stoneCircle[index]);

                }
                Vector3 customSpawnpoint = Game.instance.m_playerProfile.GetCustomSpawnPoint();

                Bed haveBed = verifyBedNearby(customSpawnpoint, 5f);

                if (haveBed == null && dead == true)
                {
                    __instance.transform.position = worldSpawnSetted;
                    dead = false;
                }


                //KG is beauty

            }

            static void Prefix(Player __instance)
            {
                Debug.Log("iniciando debug...");
                Query(__instance);
                Debug.Log("finalizando debug...");


            }
        }

    }

}