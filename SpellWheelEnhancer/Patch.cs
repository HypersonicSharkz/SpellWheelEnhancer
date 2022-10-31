using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using HarmonyLib;

namespace SpellWheelEnhancer
{
    [HarmonyPatch(typeof(WheelMenu), "GetOrbLocalPosition")]
    class Patch
    {
        static bool Prefix(ref Vector3 __result, List<WheelMenu.Orb> ___orbs, WheelMenu __instance, int orbIndex, float startAngle, float maxAngle, bool uniform = true, bool revert = false)
        {
            float num2;
            if (uniform)
            {
                float num = maxAngle / Mathf.Clamp(___orbs.Count, 0, 8);
                num2 = startAngle + num * (float)orbIndex;
            }
            else
            {
                float num = maxAngle / (float)(___orbs.Count + 1);
                num2 = startAngle - maxAngle / 2f + num * (float)(orbIndex + 1);
            }
            if (revert)
            {
                num2 = -num2;
            }

            float moveOut = Mathf.Clamp((Mathf.Floor(orbIndex / 8) * 0.5f) + 1, 1, Mathf.Infinity);
            float addAng;

            if (Mathf.Floor(orbIndex / 8) % 2 != 0 && Mathf.Floor(orbIndex / 8) != 0)
            {
                addAng = 22.5f;
            }
            else
            {
                addAng = 0f;
            }

            __result = Quaternion.AngleAxis(num2 + addAng, Vector3.forward) * Vector3.up * moveOut * __instance.orbRadius;
            return false;
        }
    }

    [HarmonyPatch(typeof(WheelMenu), "OnOrbSelected")]
    class Patch2
    {

        static bool Prefix(WheelMenu.Orb orb, bool active)
        {
            if (active)
            {
                TextMesh orbText = new GameObject("orbText").AddComponent<TextMesh>();

                orbText.transform.parent = orb.transform;
                orbText.transform.localEulerAngles = new Vector3(0, 0, 0);
                orbText.transform.localPosition = Vector3.zero;

                orbText.fontSize = SpellWheelGlobalVariables.fontSize;
                orbText.transform.localScale /= 160;


                orbText.alignment = TextAlignment.Center;
                orbText.anchor = TextAnchor.MiddleCenter;

                ContainerData data = Catalog.GetData<ContainerData>("PlayerDefault", true);
                foreach (ContainerData.Content content in Player.currentCreature.container.contents)
                {
                    if (orb.linkedObject == content)
                    {
                        orbText.text = content.itemData.displayName;
                        orbText.color = Color.white;

                        break;
                    }
                }
            }
            else
            {
                TextMesh textMesh = orb.transform.gameObject.GetComponentInChildren<TextMesh>();
                if (textMesh)
                {
                    GameObject.Destroy(textMesh);
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(WheelMenu), "Hide")]
    class Patch3
    {
        static bool Prefix(WheelMenu __instance)
        {
            TextMesh textMesh = __instance.gameObject.GetComponentInChildren<TextMesh>();
            if (textMesh)
            {
                GameObject.Destroy(textMesh);
            }
            return true;
        }
    }
}
