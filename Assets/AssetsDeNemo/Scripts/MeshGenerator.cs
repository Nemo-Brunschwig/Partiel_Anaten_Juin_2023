using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public enum Shape
{
    HighQuad,
    LowQuad,
    Side,
    BigCorner,
    LittleCorner
}

public class MeshGenerator : EditorWindow
{
    public Shape shape;

    private Vector3[] vertices;
    private int[] triangles;
    private Mesh mesh;

    [MenuItem("Window/Mesh Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MeshGenerator));
    }

    [System.Obsolete]
    void OnGUI()
    {
        GUILayout.Label("Mesh Settings", EditorStyles.boldLabel);

        // Code pour afficher les champs de saisies des variables
        shape = (Shape)EditorGUILayout.EnumPopup("Shape", shape);
        //mat = (Material)EditorGUILayout.ObjectField("material", mat, typeof(Material));
        if (GUILayout.Button("Build Mesh"))
        {
            GenerateMesh();
        }
    }

    public void GenerateMesh()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.name = shape.ToString();

        switch (shape)
        {
            case Shape.LittleCorner:
                LittleCorner();
                break;
            case Shape.BigCorner:
                BigCorner();
                break;
            case Shape.Side:
                Side();
                break;
            case Shape.LowQuad:
                LowQuad();
                break;
            case Shape.HighQuad:
                HighQuad();
                break;
        }

        UpdateMesh();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("White", typeof(Material));
    }

    public void UpdateMesh()
    {
        mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public void HighQuad()
    {
        vertices = new Vector3[]
        {
            new Vector3(-.5f, 0, -.5f),
            new Vector3(-.5f, 0, .5f),
            new Vector3(.5f, 0, .5f),
            new Vector3(.5f, 0, -.5f)
        };

        triangles = new int[]
        {
            0,1,2,
            2,3,0
        };
    }

    public void LowQuad()
    {
        HighQuad();
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y -= .5f;
        }
    }

    public void Side()
    {
        vertices = new Vector3[]
        {
            new Vector3(-.5f, -.5f, -.5f),
            new Vector3(-.5f, 0, 0),
            new Vector3(-.5f, 0, .5f),
            new Vector3(.5f, 0, .5f),
            new Vector3(.5f, 0, 0),
            new Vector3(.5f, -.5f, -.5f),
        };

        triangles = new int[]
        {
            0, 1, 4,
            4, 5, 0,
            1, 2, 3,
            3, 4, 1
        };
    }

    public void BigCorner()
    {
        vertices = new Vector3[]
        {
            new Vector3(-.5f, -.5f, -.5f),
            new Vector3(-.5f, 0, 0),
            new Vector3(-.5f, 0, .5f),
            new Vector3(0, 0, .5f),
            new Vector3(.5f,  -.5f, .5f),
            new Vector3(.5f, -.5f, -.5f),
            new Vector3(0, 0, 0),
        };

        triangles = new int[]
        {
            0,1,6,
            6,5,0,
            1,2,3,
            3,6,1,
            3,4,5,
            5,6,3
        };
    }

    public void LittleCorner()
    {
        vertices = new Vector3[]
        {
            new Vector3(-.5f, 0, -.5f),
            new Vector3(-.5f, 0, .5f),
            new Vector3(0, 0, .5f),
            new Vector3(.5f,  0, .5f),
            new Vector3(.5f,  0, 0),
            new Vector3(.5f, -.5f, -.5f),
            new Vector3(0, 0, -.5f),
            new Vector3(0, 0, 0),
        };

        triangles = new int[]
        {
            0,1,2,
            2,6,0,
            2,3,4,
            4,7,2,
            4,5,7,
            5,6,7
        };
    }
}
