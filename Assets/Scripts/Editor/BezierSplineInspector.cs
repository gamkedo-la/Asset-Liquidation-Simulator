﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(BezierSpline))]

public class BezierSplineInspector : Editor
{

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const int stepsPerCurve = 10;
    private const float directionScale = 2f;

    private const float handleSize = 0.1f;
    private const float pickSize = 0.15f;


    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.black;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

            p0 = p3;
        }

        ShowDirections();
    }

    public override void OnInspectorGUI()
    {
        spline = target as BezierSpline;

        int index = spline.GetSelectedIndex();

        if (index >= 0 && index < spline.ControlPointCount)
        { DrawSelectedPointInspector(index); Debug.Log("After Draw Call" + index); }

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }

        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }

    public void DrawSelectedPointInspector(int index)
    {
        GUILayout.Label("Selected Point: " + index.ToString());
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(index));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(index, point);
        }

        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode",
                                                                                        spline.GetControlPointMode(index));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(index, mode);
            EditorUtility.SetDirty(spline);
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);

        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; ++i)
        {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));

        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        float size = HandleUtility.GetHandleSize(point);

        if (index == 0)
        {
            size *= 2f;
        }

        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.CubeHandleCap))
        {
            spline.SetSelectedIndex(index);
            Repaint();
            Debug.Log("PRess" + spline.GetSelectedIndex());
        }


        if (spline.GetSelectedIndex() == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }

        return point;
    }

}
