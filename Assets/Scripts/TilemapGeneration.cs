using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGeneration : MonoBehaviour
{
    public GameObject Walls;
    public GameObject Floor;
    public GameObject [] Platforms;
    public int gamesize = 17;
    public float MinHeightIncrease = 4f;
    public float MaxHeightIncrease = 7f;
    public int MinPlatformLength= 5;
    public int MaxPlatformLength = 7;
    public int DistanceToAddPlatform = 20;
    public int DistanceToRemovePlatform = 30;
    public Transform PlayerLocation;

    [SerializeField] private LayerMask m_WhatIsGround;
    private int PlatformSize;
    private int PlatformAddHeight;
    private int PlatformXLocation;
    private int LastPlatformHeight = 0;
    private Stack<Vector3> DeletedPlatforms = new Stack<Vector3>();
    private LinkedListNode<Vector3> OldestActivePlatformPos;
    private LinkedListNode<Vector3> NewestActivePlatformPos;
    private int NewestActiveID;
    private int OldestActiveID;
    private LinkedList<Vector3> AllPlatformsPos = new LinkedList<Vector3>();
    private LinkedList<int> AllPlatformSize = new LinkedList<int>();
    private LinkedListNode<int> OldestSize;
    private LinkedListNode<int> NewestSize;
    private LinkedList<GameObject> ActiveWalls = new LinkedList<GameObject>();
    private int WallHeight = 30;
    private int WallRendererOutDist = 60;
    private GameObject destroy;
    private GameObject newPlatform;
    private int XOffset = 13;
    private int YOffset = -5;
    private int NextID = 0;

    private Vector3 PosToSummon;
    // Start is called before the first frame update
    void Start()
    {
        newPlatform = Instantiate(Floor);
        newPlatform.AddComponent<PlatformID>();
        newPlatform.GetComponent<PlatformID>().ID = NextID;
        NextID++;
        newPlatform.transform.SetParent(this.transform);
        for (int i = 1; i < 10; i++)
            SummonPlatform();
        newPlatform = Instantiate(Walls, new Vector3(0,0,0), Quaternion.identity);
        newPlatform.transform.SetParent(this.transform);
        ActiveWalls.AddLast(newPlatform);
        newPlatform = Instantiate(Walls, new Vector3(0, WallHeight, 0), Quaternion.identity);
        newPlatform.transform.SetParent(this.transform);
        ActiveWalls.AddLast(newPlatform);
        OldestActivePlatformPos = AllPlatformsPos.First;
        NewestActivePlatformPos = AllPlatformsPos.Last;
        OldestSize = AllPlatformSize.First;
        NewestSize = AllPlatformSize.Last;
        NewestActiveID = NextID - 1;
        OldestActiveID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(AllPlatformsPos.Last.Value.y < PlayerLocation.position.y + 20)
            SummonPlatform();
        PlatformsRenderingSystem();
        WallRenderer();
    }

    public void PlatformsRenderingSystem()
    {
        while (NewestActivePlatformPos.Value.y >= PlayerLocation.position.y + DistanceToRemovePlatform)
        {
            RaycastHit2D platform = Physics2D.Raycast(new Vector3(NewestActivePlatformPos.Value.x + XOffset, NewestActivePlatformPos.Value.y + YOffset, 0), (Vector2.up + Vector2.right).normalized, 2f, m_WhatIsGround);
            if (platform.collider == null)
            {
                Debug.Log("Cant Find Platform, go to line 69 in TilemapGeneration and fix it!");
                break;
            }
            if(platform.collider.gameObject.gameObject.name == "Tilemap")
                destroy = platform.collider.gameObject.transform.parent.gameObject;
            else
                destroy = platform.collider.gameObject;
            DestroyImmediate(destroy);
            NewestActiveID--;
            if (NewestActivePlatformPos.Previous != null)
            {
                NewestActivePlatformPos = NewestActivePlatformPos.Previous;
                NewestSize = NewestSize.Previous;
            }
            else
                break;
        }
        while (NewestActivePlatformPos.Next != null && NewestActivePlatformPos.Next.Value.y < PlayerLocation.position.y + DistanceToAddPlatform)
        {
            newPlatform = Instantiate(Platforms[NewestSize.Next.Value - 1], NewestActivePlatformPos.Next.Value, Quaternion.identity);
            newPlatform.transform.SetParent(this.transform);
            NewestActiveID++;
            newPlatform.AddComponent<PlatformID>();
            newPlatform.GetComponent<PlatformID>().ID = NewestActiveID;
            NewestSize = NewestSize.Next;
            NewestActivePlatformPos = NewestActivePlatformPos.Next;
        }
        while (OldestActivePlatformPos.Value.y <= PlayerLocation.position.y - DistanceToRemovePlatform)
        {
            RaycastHit2D platform = Physics2D.Raycast(new Vector3(OldestActivePlatformPos.Value.x + XOffset, OldestActivePlatformPos.Value.y + YOffset, 0), (Vector2.up + Vector2.right).normalized, 2f, m_WhatIsGround);
            if (platform.collider == null)
            {
                Debug.Log("Cant Find Platform, go to line 92 in TilemapGeneration and fix it!");
                break;
            }
            if (platform.collider.gameObject.gameObject.name == "Tilemap")
                destroy = platform.collider.gameObject.transform.parent.gameObject;
            else
                destroy = platform.collider.gameObject;
            DestroyImmediate(destroy);
            OldestActiveID++;
            if (OldestActivePlatformPos.Next != null)
            {
                OldestActivePlatformPos = OldestActivePlatformPos.Next;
                OldestSize = OldestSize.Next;
            }
            else
                break;
        }
        while (OldestActivePlatformPos.Previous != null && OldestActivePlatformPos.Previous.Value.y > PlayerLocation.position.y - DistanceToAddPlatform)
        {
            newPlatform = Instantiate(Platforms[OldestSize.Previous.Value - 1], OldestActivePlatformPos.Previous.Value, Quaternion.identity);
            newPlatform.transform.SetParent(this.transform);
            OldestActiveID--;
            newPlatform.AddComponent<PlatformID>();
            newPlatform.GetComponent<PlatformID>().ID = OldestActiveID;
            OldestSize = OldestSize.Previous;
            OldestActivePlatformPos = OldestActivePlatformPos.Previous;
        }
    }

    public void SummonPlatform()
    {
        PlatformAddHeight = (int)Mathf.Floor(Random.Range(LastPlatformHeight + MinHeightIncrease,LastPlatformHeight + MaxHeightIncrease + 1));
        PlatformSize = (int)Mathf.Floor(Random.Range(MinPlatformLength, MaxPlatformLength + 1));
        PlatformXLocation = (int)Mathf.Floor(Random.Range(0, gamesize - PlatformSize));
        PosToSummon = new Vector3(PlatformXLocation, PlatformAddHeight, 0);
        newPlatform = Instantiate(Platforms[PlatformSize - 1], PosToSummon, Quaternion.identity);
        newPlatform.transform.SetParent(this.transform);
        newPlatform.AddComponent<PlatformID>();
        newPlatform.GetComponent<PlatformID>().ID = NextID;
        NextID++;
        AllPlatformsPos.AddLast(PosToSummon);
        AllPlatformSize.AddLast(PlatformSize);
        LastPlatformHeight = PlatformAddHeight;
    }

    public void SummonPlatform(Vector3 pos)
    {
        newPlatform = Instantiate(Platforms[PlatformSize], pos, Quaternion.identity);
        newPlatform.transform.SetParent(this.transform);
        newPlatform.AddComponent<PlatformID>();
        newPlatform.GetComponent<PlatformID>().ID = NextID;
        NextID++;
    }

    public void WallRenderer()
    {
        while (PlayerLocation.position.y > ActiveWalls.Last.Value.transform.position.y)
        {
            newPlatform = Instantiate(Walls, new Vector3(0, ActiveWalls.Last.Value.transform.position.y + WallHeight, 0), Quaternion.identity);
            newPlatform.transform.SetParent(this.transform);
            ActiveWalls.AddLast(newPlatform);
        }
        while (PlayerLocation.position.y < ActiveWalls.First.Value.transform.position.y + WallRendererOutDist - 10 && ActiveWalls.First.Value.transform.position.y - WallHeight > -10)
        {
            newPlatform = Instantiate(Walls, new Vector3(0, ActiveWalls.First.Value.transform.position.y - WallHeight, 0), Quaternion.identity);
            newPlatform.transform.SetParent(this.transform);
            ActiveWalls.AddFirst(newPlatform);
        }
        while (PlayerLocation.position.y > ActiveWalls.First.Value.transform.position.y + WallRendererOutDist)
        {
            Destroy(ActiveWalls.First.Value);
            ActiveWalls.RemoveFirst();
        }
        while (PlayerLocation.position.y < ActiveWalls.Last.Value.transform.position.y - WallRendererOutDist)
        {
            Destroy(ActiveWalls.Last.Value);
            ActiveWalls.RemoveLast();
        }
    }
}
