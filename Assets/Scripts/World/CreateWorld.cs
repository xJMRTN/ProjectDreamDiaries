using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed  class CreateWorld : MonoBehaviour
{
private static CreateWorld instance = new CreateWorld();

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
    [SerializeField] float treeAmount;
    [SerializeField] float SpawnRadius;


    [SerializeField] GameObject[] gameLoopObjects;
    [SerializeField] GameObject[] objectiveObjects;
    public int amountOfWinConditions;
    public bool spawnObjectiveObjects;
    public int idOfObject;
    public bool spawnGhoul;
    public int amountOfGhouls;
    public bool timer;


    Vector3 spawnPoint;

    public void Awake(){
        if(instance == null) {
            instance = this;        
        }
    }

    static CreateWorld(){
    }

    private CreateWorld(){

    }

    public static CreateWorld Instance{
        get{return instance;}   
    }

    void Start(){
         mesh = new Mesh();
         mc = GetComponent<MeshCollider>();
         spawnPoint = new Vector3(WorldSize.x/2f,50f,WorldSize.y/2f);
         terrainTexture = new Texture2D(WorldSize.x, WorldSize.y);
         meshRenderer = GetComponent<MeshRenderer>();
         GetComponent<MeshFilter>().mesh = mesh;
         
         
         CreateShape();
         UpdateMesh();
        GetComponent<NavMeshSurface>().BuildNavMesh();
        SpawnTrees();
        SpawnGameLoop();
         GameEffects.Instance.StartEffects();

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
        for(int x = 0; x <= treeAmount; x++){
            Vector3 pos = FindPos();
            if(pos != Vector3.zero) Spawn(pos);
        }
    }

    void SpawnGameLoop()
    {
        SpawnObject(gameLoopObjects[0]);
        if (spawnGhoul)
        {
            for (int x = 0; x < amountOfGhouls; x++)
            {
                SpawnObject(gameLoopObjects[1]);
            }
        }
        if (spawnObjectiveObjects)
        {
            SpawnObject(gameLoopObjects[2]);
            for (int x = 0; x < amountOfWinConditions; x++)
            {
                SpawnObject(objectiveObjects[idOfObject]);
            }
        }
        SpawnObject(gameLoopObjects[3]);
    }

    public void SpawnObject(GameObject _object){
        Vector3 pos = FindPos();
        if(pos != Vector3.zero) Spawn(_object, pos);
    }

    Vector3 FindPos(){
        Vector3 ItemPosition = new Vector3();
        bool ready = false;
        int x = 0;
        while(!ready){
            Vector3 topPosition = (Vector3)Random.insideUnitCircle * SpawnRadius;
             topPosition = new Vector3 (topPosition.x + spawnPoint.x, spawnPoint.y, topPosition.y+ spawnPoint.z);  
            Vector3 placePosition = UtilityManager.Instance.ShootRayCastDown(topPosition);
            if(placePosition != Vector3.zero) {
                ready = true;
                ItemPosition = placePosition;
            }
            x++;
            if( x > 50) return Vector3.zero;
        }
        return ItemPosition;
    }

    void Spawn(Vector3 pos){
        GameObject go = Instantiate(trees[Random.Range(0, trees.Length)], pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
    }

     void Spawn(GameObject _object, Vector3 pos){
        GameObject go = Instantiate(_object, pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
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