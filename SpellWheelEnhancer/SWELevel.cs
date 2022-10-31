using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using HarmonyLib;
using System.Collections;

namespace SpellWheelEnhancer
{
    public static class SpellWheelGlobalVariables
    {
        public static int fontSize;
    }

    public class SWELevel : LevelModule
    {
        public int fontSize = 120;
        public Harmony harmony;

        public override IEnumerator OnLoadCoroutine()
        {
            yield return base.OnLoadCoroutine();

            SpellWheelGlobalVariables.fontSize = fontSize;

            try
            {
                harmony = new Harmony("SpellWeelEnhancer");
                harmony.PatchAll();
                Debug.Log("Spell Enhancer Loaded");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}
