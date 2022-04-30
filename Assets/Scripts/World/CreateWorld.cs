using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{
    Mesh mesh;
    MeshCollider mc;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;
    MeshRenderer meshRenderer;
    Texture2D terrainTexture;

    [SerializeField] Vector2Int WorldSize;
    [SerializeField] List<NoiseLayer> noiseLayers = new List<NoiseLayer>();

    [SerializeField] GameObject[] trees;

    void Start(){
         mesh = new Mesh();
         mc = GetComponent<MeshCollider>();
         
         terrainTexture = new Texture2D(WorldSize.x, WorldSize.y);
         meshRenderer = GetComponent<MeshRenderer>();
         GetComponent<MeshFilter>().mesh = mesh;
         CreateShape();
         UpdateMesh();
         SpawnTrees();
    }

    void CreateShape(){
        uv = new Vector2[(WorldSize.x + 1) * (WorldSize.y + 1)];
        vertices = new Vector3[(WorldSize.x + 1) * (WorldSize.y + 1)];
        triangles = new int[WorldSize.x * WorldSize.y * 6];
        int vert = 0;
        int tris = 0;
    
        for (int i = 0, z = 0; z <= WorldSize.y; z++)
        {
            for (int x= 0; x <= WorldSize.x ;x++)
            {            
                uv[i] = new Vector2(x / (float)WorldSize.x, z / (float) WorldSize.y);
                vertices[i] = new Vector3(x, 0, z);

                float elevation = 0;
                for (int l = 0; l < noiseLayers.Count; l++)
                {
                    elevation += noiseLayers[l].Evaluate(x / (float)WorldSize.x, z / (float)WorldSize.y);

                }

                vertices[i].y = elevation;

                i++;
              
                if(x != WorldSize.x && z != WorldSize.y){            
                    triangles[0 + tris] = vert + 0;
                    triangles[1 + tris] = vert + WorldSize.x + 1;
                    triangles[2 + tris] = vert + 1;
                    triangles[3 + tris] = vert + 1;
                    triangles[4 + tris] = vert + WorldSize.x + 1;
                    triangles[5 + tris] = vert + WorldSize.x + 2;
                    vert++;
                    tris += 6;
                }
            }
            if(z != WorldSize.y) vert++;
        }
    }

    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        terrainTexture.filterMode = FilterMode.Bilinear;
        terrainTexture.wrapMode = TextureWrapMode.Clamp;
        terrainTexture.Apply();
        meshRenderer.material.mainTexture = terrainTexture;
        mesh.RecalculateNormals();
        mc.sharedMesh = mesh;
    }

    void SpawnTrees(){
        
    }
}


[System.Serializable]
public class NoiseLayer
{ 
    [SerializeField] float noisePower = 1;   
    [SerializeField] Vector2 noiseOffset;    
    [SerializeField] float noiseScale = 1;

    public float Evaluate(float x, float y)
    {
        float noiseXCoord = noiseOffset.x + x * noiseScale;
        float noiseYCoord = noiseOffset.y + y * noiseScale;
        return (Mathf.PerlinNoise(noiseXCoord, noiseYCoord)) * noisePower;
    }
}