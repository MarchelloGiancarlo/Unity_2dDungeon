using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Threading.Tasks;

[Serializable]
public class RoomEnemies {
	[SerializeField]
	public List<List<GameObject>> enemyWaves;
}

public class RoomModule : MonoBehaviour {
	enum RoomState {
		uncomplete, activateRoom, waving, complete
	}
	public RoomProperty roomProperties;
	public DungeonAssetModule dungeonAsset;
	public RoomEnemies roomEnemies;
	[SerializeField] private RoomState roomState = RoomState.uncomplete;
	private int waveNumber;
	private List<GameObject> enemyInTheRoom;
	private List<GameObject> doorsObjects;
	private void Awake() {
		enemyInTheRoom = new List<GameObject>();
		doorsObjects = new List<GameObject>();
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerModule>()) {
			roomState = RoomState.activateRoom;
		}
	}
	private void Update() {
		if (roomProperties.category == RoomProperty.RoomCategory.enemyRoom) {
			switch (roomState) {
				case RoomState.uncomplete:
					break;
				case RoomState.activateRoom:
					//Close all the doors
					doorsObjects.ForEach(door => door.GetComponent<DoorModule>().closeDoor());
					GetComponent<BoxCollider2D>().enabled = false;
					waveNumber = 0;
					roomState = RoomState.waving;
					break;
				case RoomState.waving:
					if (!thereAreEnemiesInRoom()) {
						if (waveNumber < roomEnemies.enemyWaves.Count) {
							launchWave(waveNumber);
							waveNumber++;
						} else {
							roomState = RoomState.complete;
						}
					}
					break;
				case RoomState.complete:
					//Open all the doors
					doorsObjects.ForEach(door => door.GetComponent<DoorModule>().openDoor());
					break;
			}
		}
	}
	public void generateBasicRoom() {
		if (roomProperties.category == RoomProperty.RoomCategory.enemyRoom) {
			GetComponent<BoxCollider2D>().size =
				new Vector2(roomProperties.basicDim.x - 2, roomProperties.basicDim.y - 2);
			GetComponent<BoxCollider2D>().offset =
				new Vector2((float)roomProperties.basicDim.x / 2, (float)roomProperties.basicDim.y / 2);
		} else {
			GetComponent<BoxCollider2D>().enabled = false;
		}
		DungeonDrawer.drawRoom(dungeonAsset,
			new Vector2Int((int)transform.position.x, (int)transform.position.y),
			roomProperties, Utils.GlobalDirection.NORTH);
		generateDoors();
	}
	private void generateDoors() {
		for (int i = 0; i < roomProperties.doorsPosition.Count; i++) {
			Vector2 centerPoint = new Vector2();
			Vector2Int doorPos = roomProperties.doorsPosition[i];
			Utils.Orientation orientation = Utils.Orientation.vertical;
			if (doorPos.x == 0) {
				centerPoint = new Vector2(0.5f, doorPos.y);
				orientation = Utils.Orientation.vertical;
			} else if (doorPos.y == 0) {
				centerPoint = new Vector2(doorPos.x, 0.5f);
				orientation = Utils.Orientation.horizontal;
			} else if (doorPos.x == roomProperties.basicDim.x - 1) {
				orientation = Utils.Orientation.vertical;
				centerPoint = new Vector2(roomProperties.basicDim.x - 0.5f, doorPos.y);
			} else if (doorPos.y == roomProperties.basicDim.y - 1) {
				centerPoint = new Vector2(doorPos.x, roomProperties.basicDim.y - 0.5f);
				orientation = Utils.Orientation.horizontal;
			} else
				Debug.LogError("Error on create Door");
			GameObject door = Instantiate(dungeonAsset.doorPrefab, transform.position + (Vector3)centerPoint,
				Quaternion.Euler(0, 0, 0));
			door.transform.parent = transform;
			door.GetComponent<DoorModule>().setProperties(dungeonAsset, orientation);
			doorsObjects.Add(door);
		}
	}
	private void launchWave(int waveIndex) {
		foreach (GameObject enemy in roomEnemies.enemyWaves[waveNumber]) {
			enemyInTheRoom.Add(Instantiate(enemy, getValidSpawnPoint(), Quaternion.Euler(0, 0, 0)));
		}
	}
	private bool thereAreEnemiesInRoom() {
		if (enemyInTheRoom.Count != 0)
			foreach (GameObject enemy in enemyInTheRoom) {
				if (enemy != null) {
					return true;
				}
			}
		return false;
	}
	// void lauchNextWave() {
	// 	if (waveNumber >= roomEnemies.enemyWaves.Count) {
	// 		roomState = RoomState.complete;
	// 		foreach (Transform child in transform) {
	// 			if (child.GetComponent<DoorModule>() != null)
	// 				child.GetComponent<DoorModule>().openDoor();
	// 		}
	// 		return;
	// 	}
	// 	Debug.Log("Wave " + waveNumber);
	// 	foreach (GameObject enemy in roomEnemies.enemyWaves[waveNumber]) {
	// 		enemyInTheRoom.Add(Instantiate(enemy, getValidSpawnPoint(), Quaternion.Euler(0, 0, 0)));
	// 	}
	// 	waveNumber++;
	// }
	private Vector2 getValidSpawnPoint() {
		return Utils.getRandomClearPointInRect(new Rect(transform.position.x, transform.position.y,
			roomProperties.basicDim.x, roomProperties.basicDim.y), 1);
	}
}
