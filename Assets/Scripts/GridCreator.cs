using UnityEngine;
using UnityEngine.UI;


public class GridCreator : MonoBehaviour
{
    public Vector3 size = new Vector3(1, 1, 1);
    public Vector3 resolution = new Vector3(10, 10, 10);
    public Material outline_material = null;

    public GridPoint[,,] pts = null; // a 3-dimensional array of points

    private GameObject outline;

    public Transform origin;

    void Start()
    {
        Debug.Log("Creating Grid");

        // initialize empty list of grid pts
        pts = new GridPoint[(int)resolution.x, (int)resolution.y, (int)resolution.z];


        // get origin's transform
        origin = this.transform.Find("Origin");

        // Create outline cube
        outline = GameObject.CreatePrimitive(PrimitiveType.Cube);
        outline.name = string.Format("Outline");
        outline.transform.parent = origin;
        outline.transform.localPosition = new Vector3(0.5f * size.x, 0.5f * size.y, 0.5f * size.z);
        outline.transform.localScale = new Vector3(size.x, size.y, size.z);
        outline.GetComponent<MeshRenderer>().material = outline_material;

        // fill in pts information in the grid
        float dx = size.x / (resolution.x - 1);
        float dy = size.y / (resolution.y - 1);
        float dz = size.z / (resolution.z - 1);

        for (int z = 0; z < (int)resolution.z; z++)
        {
            for (int y = 0; y < (int)resolution.y; y++)
            {
                for (int x = 0; x < (int)resolution.x; x++)
                {
                    Vector3 pos = new Vector3(origin.transform.position.x + x * dx, origin.transform.position.y + y * dy, origin.transform.position.z + z * dz);
                    pts[x, y, z] = new GridPoint(pos);

                }
            }
        }
    }


    void Update()
    {
        // call update of each GridPoint
        for (int z = 0; z < (int)resolution.z; z++)
            for (int y = 0; y < (int)resolution.y; y++)
                for (int x = 0; x < (int)resolution.x; x++)
                    pts[x, y, z].Update();
    }



    void OnDrawGizmos()
    {
        // Draw a yellow cube at the transform position of the origin (only on the scene, not rendered in play time)
        Gizmos.color = Color.yellow;
        Transform origin = this.transform.Find("Origin");
        Gizmos.DrawWireCube(origin.position + new Vector3(0.5f * size.x, 0.5f * size.y, 0.5f * size.z), new Vector3(size.x, size.y, size.z));
    }


}
