using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class DungeonGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase floorTile;
    public TileBase wallTile;
   

    public int dungeonWidth = 20;
    public int dungeonHeight = 20;

    public int minRoomSize = 3;
    public int maxRoomSize = 6;
    public int numRooms = 5;



    public GameObject[] spawnLoot; // Add the prefab for the object you want to spawn
    public GameObject[] spawnEnemies; // Add the prefab for the object you want to spawn

    public GameObject[] bugs;


    public int numLootToSpawn = 3; // Number of objects to spawn on walls
    public int numEnemiesToSpawn = 3;
  
    public int bugAmt = 8;

    public GameObject startPointPrefab; // Start point prefab
    public GameObject endPointPrefab;   // End point prefab

    private void Start()
    {
        numRooms = Random.Range(3, 8);
       
        numEnemiesToSpawn += GameManager.Instance.level / 6;
        bugAmt += GameManager.Instance.level / 3;


        GenerateDungeon();
    }


    public void GenerateDungeon()
    {
        // Initialize the dungeon with empty tiles
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        // Generate rooms
        List<Rect> rooms = new List<Rect>();

        for (int i = 0; i < numRooms; i++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize + 1);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize + 1);

            int xPos = Random.Range(1, dungeonWidth - roomWidth - 1);
            int yPos = Random.Range(1, dungeonHeight - roomHeight - 1);

            Rect newRoom = new Rect(xPos, yPos, roomWidth, roomHeight);

            // Check for overlap with existing rooms
            bool overlaps = rooms.Any(existingRoom => newRoom.Overlaps(existingRoom));
            if (!overlaps)
            {
                rooms.Add(newRoom);

                // Generate deformed room of random shape and size
                GenerateIrregularRoom(newRoom);
            }
            else
            {
                // If overlap occurs, try generating a new room
                i--;
            }
        }

        // Connect rooms with corridors
        for (int i = 1; i < rooms.Count; i++)
        {
            ConnectRooms(rooms[i - 1], rooms[i]);
        }

        // Add a single layer of walls around rooms and corridors
        AddWallsAroundRoomsAndCorridors();

        // Spawn objects on random floor tiles in both rooms and corridors
        SpawnEnemiesTiles(rooms);
        SpawnLootTiles(rooms);
        SpawnBugs(rooms);
        //SpawnObjectsTiles(rooms);

 

        SpawnStartAndEndPoints(rooms);
    }


    void GenerateIrregularRoom(Rect room)
    {
        // Randomly choose between a square and a circle
        if (Random.value > 0.5f)
        {
            // Generate square room
            for (int x = Mathf.FloorToInt(room.x); x < Mathf.FloorToInt(room.x + room.width); x++)
            {
                for (int y = Mathf.FloorToInt(room.y); y < Mathf.FloorToInt(room.y + room.height); y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }
        else
        {
            // Generate circular room
            int centerX = Mathf.FloorToInt(room.x + room.width / 2);
            int centerY = Mathf.FloorToInt(room.y + room.height / 2);
            int radius = Mathf.FloorToInt(Mathf.Min(room.width, room.height) / 2);

            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY)) <= radius)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                    }
                }
            }
        }
    }


    void ConnectRooms(Rect roomA, Rect roomB)
    {
        // Find the center of each room
        Vector3Int centerA = new Vector3Int(
        Mathf.FloorToInt(roomA.x + roomA.width / 2),
        Mathf.FloorToInt(roomA.y + roomA.height / 2),
        0
    );

        Vector3Int centerB = new Vector3Int(
        Mathf.FloorToInt(roomB.x + roomB.width / 2),
        Mathf.FloorToInt(roomB.y + roomB.height / 2),
        0
    );

        // Generate a random corridor width (adjust the value as needed)
        int corridorWidth = Random.Range(2, 5); // You can adjust the range for the corridor width

        // Connect the rooms with a corridor
        if (Random.value < 0.5f)
        {
            // Horizontal then vertical corridor
            CreateHorizontalCorridor(centerA.x, centerB.x, centerA.y, corridorWidth);
            CreateVerticalCorridor(centerA.y, centerB.y, centerB.x, 3); // Set the vertical corridor height to 3
        }
        else
        {
            // Vertical then horizontal corridor
            CreateVerticalCorridor(centerA.y, centerB.y, centerA.x, corridorWidth);
            CreateHorizontalCorridor(centerA.x, centerB.x, centerB.y, 3); // Set the horizontal corridor width to 3
        }
    }

    void CreateHorizontalCorridor(int startX, int endX, int y, int width)
    {
        int direction = (startX < endX) ? 1 : -1;

        for (int x = startX; x != endX + direction; x += direction)
        {
            for (int w = -width / 2; w <= width / 2 + 1; w++) // Increase the width by 2
            {
                tilemap.SetTile(new Vector3Int(x, y + w, 0), floorTile);
            }
        }
    }

    void CreateVerticalCorridor(int startY, int endY, int x, int height)
    {
        int direction = (startY < endY) ? 1 : -1;

        for (int y = startY; y != endY + direction; y += direction)
        {
            for (int h = -height / 2; h <= height / 2; h++)
            {
                tilemap.SetTile(new Vector3Int(x + h, y, 0), floorTile);
            }
        }
    }
    void AddWallsAroundRoomsAndCorridors()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                if (tilemap.GetTile(new Vector3Int(x, y, 0)) == floorTile)
                {
                    for (int w = -1; w <= 1; w++)
                    {
                        for (int h = -1; h <= 1; h++)
                        {
                            if (w == 0 && h == 0)
                            {
                                continue; // Skip the floor tile itself
                            }

                            int wallHeight = (h == -1) ? 1 : 3;

                            // Check if it's a taller wall (top wall of a room or corridor)
                            bool isTallerWall = (h == 1 && tilemap.GetTile(new Vector3Int(x + w, y + h - 1, 0)) == floorTile);

                            if (tilemap.GetTile(new Vector3Int(x + w, y + h, 0)) == null)
                            {
                                for (int i = 0; i < wallHeight; i++)
                                {
                                   
                                   tilemap.SetTile(new Vector3Int(x + w, y + h + i, 0), wallTile);
                                    
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    void SpawnLootTiles(List<Rect> rooms)
    {
        // Get all floor tile positions
        List<Vector3Int> floorTilePositions = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var position in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position) && tilemap.GetTile(position) == floorTile)
            {
                floorTilePositions.Add(position);
            }
        }

        // Shuffle the floor tile positions
        floorTilePositions = floorTilePositions.OrderBy(x => Random.value).ToList();

        // Spawn objects on a few random floor tiles in both rooms and corridors
        for (int i = 0; i < Mathf.Min(numLootToSpawn, floorTilePositions.Count); i++)
        {
            Vector3Int spawnPosition = floorTilePositions[i];

            Instantiate(spawnLoot[Random.Range(0, spawnLoot.Length)], tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);
        }
    }

    void SpawnBugs(List<Rect> rooms)
    {
        // Get all floor tile positions
        List<Vector3Int> floorTilePositions = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var position in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position) && tilemap.GetTile(position) == floorTile)
            {
                floorTilePositions.Add(position);
            }
        }

        // Shuffle the floor tile positions
        floorTilePositions = floorTilePositions.OrderBy(x => Random.value).ToList();

        // Spawn objects on a few random floor tiles in both rooms and corridors
        for (int i = 0; i < Mathf.Min(bugAmt, floorTilePositions.Count); i++)
        {
            Vector3Int spawnPosition = floorTilePositions[i];

            Instantiate(bugs[Random.Range(0, 2)], tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);
        }
    }

    void SpawnEnemiesTiles(List<Rect> rooms)
    {
        // Get all floor tile positions
        List<Vector3Int> floorTilePositions = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var position in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position) && tilemap.GetTile(position) == floorTile)
            {
                floorTilePositions.Add(position);
            }
        }

        // Shuffle the floor tile positions
        floorTilePositions = floorTilePositions.OrderBy(x => Random.value).ToList();

        // Spawn objects on a few random floor tiles in both rooms and corridors
        for (int i = 0; i < Mathf.Min(numEnemiesToSpawn, floorTilePositions.Count); i++)
        {
            Vector3Int spawnPosition = floorTilePositions[i];

            Instantiate(spawnEnemies[Random.Range(0, spawnEnemies.Length)], tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);
        }
    }






    void SpawnStartAndEndPoints(List<Rect> rooms)
    {
        // Shuffle the rooms
        rooms = rooms.OrderBy(x => Random.value).ToList();

        // Spawn start point at the center of the first room
        Rect firstRoom = rooms[0];
        Vector3Int startSpawnPosition = new Vector3Int(
            Mathf.FloorToInt(firstRoom.x + firstRoom.width / 2),
            Mathf.FloorToInt(firstRoom.y + firstRoom.height / 2),
            0
        );
        Instantiate(startPointPrefab, tilemap.GetCellCenterWorld(startSpawnPosition), Quaternion.identity);


        // Spawn end point at the center of the last room
        Rect lastRoom = rooms[rooms.Count - 1];
        Vector3Int endSpawnPosition = new Vector3Int(
            Mathf.FloorToInt(lastRoom.x + lastRoom.width / 2),
            Mathf.FloorToInt(lastRoom.y + lastRoom.height / 2),
            0
        );
        Instantiate(endPointPrefab, tilemap.GetCellCenterWorld(endSpawnPosition), Quaternion.identity);


    }
}