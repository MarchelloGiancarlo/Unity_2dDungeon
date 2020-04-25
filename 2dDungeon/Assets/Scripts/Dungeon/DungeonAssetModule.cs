using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class DungeonAssetModule : MonoBehaviour {
	[Serializable]
	public class DungeonTiles {
		public Tile wallFront_Left, wallFront_Center, wallFront_Right;
		public Tile groundBase;
		public Tile wallTop_Bottom_Left, wallTop_Bottom_Center, wallTop_Bottom_Right;
		public Tile wallTop_BottomLeft, wallTop_BottomRight, wallTop_Left, wallTop_Right;
		public Tile wallTop_BottomLeftCorner, wallTop_BottomRightCorner;
		public Tile pillar_Bottom, pillar_Middle, pillar_Top;
	}
	public Tilemap tilemapGround, tilemapWall, tilemapWallFront, tilemapCollider;
	public Tile groundTile, wallTop, wallFront, wallLeft, wallRight;
	public List<GameObject> enemies;
	public GameObject doorPrefab;
	public Sprite spriteDoorFrontOpen, spriteDoorSideOpen, spriteDoorFrontClosed, spriteDoorSideClosed;
	[SerializeField] public DungeonTiles dungeonTiles;

}
