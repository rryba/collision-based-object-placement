using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ObjectPlacerJobSystem
{
    [CustomEditor(typeof(ObjectPlacerJob))]
    public class ObjectPlacerJobEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ObjectPlacerJob myScript = (ObjectPlacerJob)target;

            if (myScript.IsWorking)
            {
                if (GUILayout.Button("Stop"))
                {
                    myScript.Stop();
                }
                
                Repaint();
            }
            else
            {
                if (GUILayout.Button("Generate"))
                {
                    if (!Application.isPlaying)
                    {
                        Debug.LogWarning("Jobs can only be generating in play mode!");
                        return;
                    }
                    myScript.Run();
                    Repaint();
                }
            }
        }
    }

    [CustomEditor(typeof(EmptyOPJ), true)]
    public class EmptyOPJEditor : ObjectPlacerJobEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EmptyOPJ myScript = (EmptyOPJ)target;

            if (!myScript.IsWorking && myScript.HasData)
            {
                if (GUILayout.Button("Save"))
                {
                    myScript.SaveJobDataAsSO();
                    Repaint();
                }

                if (GUILayout.Button("Clear"))
                {
                    myScript.ClearJobData();
                    Repaint();
                }
            }
        }
    }

    [CustomEditor(typeof(BasicOPJ), true)]
    public class BasicOPJEditor : ObjectPlacerJobEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BasicOPJ myScript = (BasicOPJ)target;

            if (!myScript.IsWorking && myScript.HasData)
            {
                if (GUILayout.Button("Save"))
                {
                    myScript.SaveJobDataAsSO();
                    Repaint();
                }

                if (GUILayout.Button("Clear"))
                {
                    myScript.ClearJobData();
                    Repaint();
                }
            }
        }
    }

    [CustomEditor(typeof(OPJLoader), true)]
    public class OPJLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            OPJLoader myScript = (OPJLoader)target;

            if (GUILayout.Button("Load Job"))
            {
                myScript.LoadJob();
                Repaint();
            }

        }
    }
}