using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class GenerateResources : MonoBehaviour
{
    public GameObject tree;
    public Transform TreesDir;
    public GameObject stone;
    public Transform StonesDir;

    public int MapSize = 30;
    public int SpawnTimeUpper = 20;
    public int SpawnTimeLower = 40;
    private int IntervalsTime;

    private bool SuccessSpawn = false;

    private GameObject newresource;
    private Collider2D resourceCollider;

    private List<Collider2D> colliders = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        IntervalsTime = UnityEngine.Random.Range(SpawnTimeUpper,SpawnTimeLower);
        Invoke("generate",IntervalsTime * Difficulty.levelIntervalRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generate()
    {
        SuccessSpawn = false;
        while(!SuccessSpawn)
        {
            SuccessSpawn = true;

            // calculate coordinate
            int Xblock = UnityEngine.Random.Range(-MapSize+1,MapSize);
            float positionX = Xblock * 0.5f;
            int Yblock = UnityEngine.Random.Range(0, MapSize - Math.Abs(Xblock));
            float positionY = Yblock * 0.5f + (-0.25f*(MapSize+1) + 0.25f*Math.Abs(Xblock));
            var position = new Vector3(positionX, positionY, 0);

            // create object
            int item = UnityEngine.Random.Range(0,2); // generate tree or stone
            if(item == 0){
                newresource = Instantiate(tree, position, Quaternion.identity, TreesDir);
            }
            else if(item == 1){
                newresource = Instantiate(stone, position, Quaternion.identity, StonesDir);
            }
            
            // check for overlap
            resourceCollider = newresource.GetComponent<PolygonCollider2D>();
            if(Physics2D.OverlapCollider(resourceCollider, new ContactFilter2D().NoFilter(), colliders) > 0)
            {
                Destroy(newresource);
                SuccessSpawn = false;
            }
        }
        Debug.Log("generate resource" + IntervalsTime);
        if (IntervalsTime <= (3 * SpawnTimeUpper + SpawnTimeLower)/4 ) {
            IntervalsTime = UnityEngine.Random.Range((SpawnTimeUpper+SpawnTimeLower)/2,SpawnTimeLower);
        }else if (IntervalsTime >= (SpawnTimeUpper + 3 * SpawnTimeLower)/4 ) {
            IntervalsTime = UnityEngine.Random.Range(SpawnTimeUpper,(SpawnTimeUpper+SpawnTimeLower)/2);
        }else {
            IntervalsTime = UnityEngine.Random.Range(SpawnTimeUpper,SpawnTimeLower);
        }
        Invoke("generate",IntervalsTime * Difficulty.levelIntervalRate);
    }
}
