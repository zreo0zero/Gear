using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SilkyRobot.GearSystem
{
    public class GearSystemTools : EditorWindow
    {
        [MenuItem("Tools/SilkyRobot/GearSystem Tools")]
        static void Init()
        {
            GearSystemTools window = GetWindow<GearSystemTools>();
            window.titleContent = new GUIContent("GearSystem Tools");
            window.Show();
        }

        Vector2 scrollPos;
        void OnGUI()
        {
            scrollPos=GUILayout.BeginScrollView(scrollPos);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent { text = "Snap", tooltip = "Snap selected to last selected object" }, GUILayout.Height(35)))
                Snap(true,true,false);

            if (GUILayout.Button(new GUIContent { text = "Match Scale" },new GUILayoutOption[] { GUILayout.Height(35), GUILayout.Width(position.width*.5f) }))
                Snap(false, false, true);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent { text = "Connect", tooltip = "Connect GearComponents in the selected order" }, GUILayout.Height(35)))
                CreateGearChain();
            if (GUILayout.Button(new GUIContent { text = "Detache" } , GUILayout.Height(35)))
                DetacheSelected();
            if (GUILayout.Button("Clear Driven", GUILayout.Height(35)))
                ClearDriven();
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent { text = "Select Driver", tooltip = "Select GearComponent that drives the last selected object" }, GUILayout.Height(35)))
                SelectDriver();
            if (GUILayout.Button(new GUIContent { text = "Select Driven", tooltip = "Select GearComponents that are driven by last selected object" }, GUILayout.Height(35)))
                SelectDriven();
            GUILayout.EndScrollView();
        }



        List<GearComponent> GetSelectedGearComponents()
        {
            List<GearComponent> list = new();

            foreach (var i in Selection.objects)
            {
                if (AssetDatabase.Contains(i))
                    continue;

                if (i is GameObject && (i as GameObject).TryGetComponent<GearComponent>(out var gear))
                    list.Add(gear);

            }
            return list;
        }

        void CreateGearChain()
        {
            List<GearComponent> list = GetSelectedGearComponents();

            if (list.Count < 2)
                return;

            Undo.RecordObjects(list.ToArray(), "Create Gear-Chain");

            for (int i = 0; i < list.Count - 1; i++)
            {
                GearComponent gc = list[i + 1];
                FindObjectsOfType<GearComponent>().Where(x => x.Driven.Contains(gc)).ToList().ForEach(x => x.RemoveDrivenComponent(gc));

                list[i].AddDrivenComponent(gc);
                gc.Evaluate(list[i]);
                EditorUtility.SetDirty(list[i]);
            }
        }

        void SelectDriven()
        {
            if (Selection.activeGameObject == null || AssetDatabase.Contains(Selection.activeGameObject))
                return;

            List<GameObject> driven = new();
            if (Selection.activeGameObject.TryGetComponent<GearComponent>(out var gear))
            {
                foreach (var i in gear.Driven)
                {
                    if (i != null)
                    {
                        driven.Add(i.gameObject);
                        EditorUtility.SetDirty(gear);
                    }
                }
                if (driven.Count > 0)
                    Selection.objects = driven.ToArray();
            }
        }

        void SelectDriver()
        {
            if (Selection.activeGameObject == null || AssetDatabase.Contains(Selection.activeGameObject))
                return;

            if (Selection.activeGameObject.TryGetComponent<GearComponent>(out var gear))
            {
                var gc = FindObjectsByType<GearComponent>(FindObjectsSortMode.None);
                foreach (var i in gc)
                {
                    if (i.Driven.Contains(gear))
                    {
                        Selection.activeObject = i.gameObject;
                        return;
                    }
                }

                var m = FindObjectsByType<Gear_Motor>(FindObjectsSortMode.None);

                foreach (var i in m)
                {
                    if (i.Driven == gear)
                    {
                        Selection.activeObject = i.gameObject;
                        return;
                    }
                }
            }
        }

        void ClearDriven()
        {
            foreach (var i in Selection.objects)
            {
                if (AssetDatabase.Contains(i))
                    continue;

                if (i is GameObject && (i as GameObject).TryGetComponent<GearComponent>(out var gear))
                {
                    gear.RemoveAllDriven();
                    EditorUtility.SetDirty(gear);
                }
            }
        }

        void DetacheSelected()
        {
            int c = 0;
            List<GearComponent> list = GetSelectedGearComponents();
            foreach (var i in list)
            {
                if (Detache(i))
                    c++;
            }
            if (c > 0)
                Debug.Log($"detached {c} GearComponents");

        }

        bool Detache(GearComponent gear)
        {
            if (gear == null)
                return false;

            var gc = FindObjectsByType<GearComponent>(FindObjectsSortMode.None);
            foreach (var i in gc)
            {
                if (i.Driven.Contains(gear))
                {
                    i.RemoveDrivenComponent(gear);
                    EditorUtility.SetDirty(i);
                    return true;
                }
            }

            var m = FindObjectsByType<Gear_Motor>(FindObjectsSortMode.None);
            foreach (var i in m)
            {
                if (i.Driven == gear)
                {
                    i.Driven = null;
                    EditorUtility.SetDirty(i);
                    return true;
                }
            }

            return false;
        }



        void Snap(bool pos, bool rot, bool scale)
        {
            List<Transform> list = new();

            foreach (var i in Selection.objects)
            {
                if (AssetDatabase.Contains(i))
                    continue;

                if (i is GameObject)
                    list.Add((i as GameObject).transform);

            }

            if (list.Count < 2)
                return;

            Undo.RecordObjects(list.ToArray(), "Snap");

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (pos)
                    list[i].position = list[^1].position;
                if (rot)
                    list[i].rotation = list[^1].rotation;
                if (scale)
                    list[i].localScale = list[^1].localScale;

                EditorUtility.SetDirty(list[i]);
            }
            Selection.activeTransform = list[0];
        }


    }
}
