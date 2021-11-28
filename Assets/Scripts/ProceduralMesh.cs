using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProceduralMesh : MonoBehaviour
{
    public Material material = null;

    private Mesh mesh = null;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private GridCreator grid = null;

    private void Generate()
    {
        GameObject go = this.gameObject;

        // --- get empty mesh
        mesh = GetMesh(go, material);
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        // --- run marching cubes
        float iso = 0.0f;
        
        MarchingCubes.Modify(grid, iso, vertices, triangles, uv);
        
        

        // --- set mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // --- set mesh transform
        transform.parent = grid.origin;

    }


    public Mesh GetMesh(GameObject go, Material material)
    {
        MeshFilter mf = go.GetComponent<MeshFilter>(); //add meshfilter component
        if (mf == null)
            mf = go.AddComponent<MeshFilter>();

        MeshRenderer mr = go.GetComponent<MeshRenderer>(); //add meshrenderer component
        if (mr == null)
            mr = go.AddComponent<MeshRenderer>();

        mr.material = material;

        //allocate mesh 
        if (Application.isEditor == true)
        {
            mesh = mf.sharedMesh;
            if (mesh == null)
            {
                mf.sharedMesh = new Mesh();
                mesh = mf.sharedMesh;
            }
        }
        else
        {
            mesh = mf.mesh;
            if (mesh == null)
            {
                mf.mesh = new Mesh();
                mesh = mf.mesh;
            }
        }
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; //increase max vertices per mesh
        mesh.name = "ProceduralMesh";

        return mesh;
    }


/*    public void TransformMesh(Transform T)
    {
        gameObject.transform.position = T.position;
        gameObject.transform.localScale = T.localScale;
        gameObject.transform.rotation = T.rotation;
    }*/


    void Start()
    {
        grid = GetComponentInParent<GridCreator>();

    }

    void Update()
    {
        if (grid != null && grid.redraw_points.Count > 0)
        {
            Generate();
            grid.redraw_points.Clear();
            Debug.Log("Generating marching cubes mesh!");
        }
    }
}
