using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

public class DungeonModule : MonoBehaviour {
	public AstarPath astar;
	public List<RoomModule> roomModules;
	public GameObject roomPrefab;
	private DungeonAssetModule dungeonAsset;
	[SerializeField] private DungeonGenerator.DGresult demoDungeon;
	private void Awake() {
		dungeonAsset = GameObject.FindObjectOfType<DungeonAssetModule>();
	}
	void Start() {
		generateDungeon();
	}
	private void generateDungeon() {
		DungeonGenerator.DGresult dungeon = demoDungeon;
		if (dungeon.rooms.Count == 0) {
			Debug.Log("No room on generated dungeon");
			return;
		}
		foreach (DungeonGenerator.DGresult.Room DGroom in dungeon.rooms) {
			GameObject roomObject = Instantiate(roomPrefab, (Vector3Int)DGroom.position, Quaternion.Euler(0, 0, 0));
			RoomModule roomModule = roomObject.GetComponent<RoomModule>();
			roomObject.transform.parent = transform;
			roomModule.roomProperties = DGroom.properties;
			roomModule.dungeonAsset = dungeonAsset;
			roomModule.roomEnemies = new RoomEnemies();
			roomModule.roomEnemies.enemyWaves = new List<List<GameObject>>();
			roomModule.roomEnemies.enemyWaves.Add(new List<GameObject> {
				dungeonAsset.enemies[0]
				 });
			roomModules.Add(roomModule);
		}
		roomModules.ForEach(room => { room.generateBasicRoom(); });
		astar.enabled = true;
		astar.Scan();
	}
}
