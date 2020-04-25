using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Threading.Tasks;

public static class DungeonDrawer {
	private enum CellState {
		emptyCell, groundCell
	}
	private static DungeonAssetModule dungeonAsset;
	public static bool drawRoom(DungeonAssetModule dungeonAsset, Vector2Int origin, RoomProperty property, Utils.GlobalDirection direction) {
		//TODO add logic for room rotation
		DungeonDrawer.dungeonAsset = dungeonAsset;
		CellState[,] cells = generateCells(property);
		//Ground and Collider draw
		try {
			for (int x = 0; x < property.basicDim.x; x++) {
				for (int y = 0; y < property.basicDim.y; y++) {
					switch (cells[x, y]) {
						case CellState.emptyCell:
							placeTileCollider(new Vector3Int(origin.x + x, origin.y + y, 0),
								dungeonAsset.dungeonTiles.groundBase);
							break;
						case CellState.groundCell:
							placeTileGround(new Vector3Int(origin.x + x, origin.y + y, 0),
								dungeonAsset.dungeonTiles.groundBase);
							break;
					}
				}
			}
			//Wall draw
			for (int x = 0; x < property.basicDim.x; x++) {
				for (int y = 0; y < property.basicDim.y; y++) {
					// switch (cells[x, y])
					// {

					// }
				}
			}
		} catch (NullReferenceException ex) {

		}
		return true;
	}
	private static CellState[,] generateCells(RoomProperty roomProperties) {
		CellState[,] cells = new CellState[roomProperties.basicDim.x, roomProperties.basicDim.y];
		for (int x = 0; x < roomProperties.basicDim.x; x++) {
			for (int y = 0; y < roomProperties.basicDim.y; y++) {
				if (x == 0 || y == 0 || x == roomProperties.basicDim.x - 1 || y == roomProperties.basicDim.y - 1)
					cells[x, y] = CellState.emptyCell;
				else
					cells[x, y] = CellState.groundCell;
			}
		}
		if (roomProperties.removedGround.Count != 0)
			foreach (RectInt rect in roomProperties.removedGround) {
				for (int x = rect.x; x < rect.x + rect.width; x++) {
					for (int y = rect.y; y < rect.y + rect.height; y++) {
						cells[x, y] = CellState.emptyCell;
					}
				}
			}
		foreach (Vector2Int doorPos in roomProperties.doorsPosition) {
			if (isValidPosition(roomProperties, new Vector2Int(doorPos.x, doorPos.y)))
				cells[doorPos.x, doorPos.y] = CellState.groundCell;
			if (isValidPosition(roomProperties, new Vector2Int(doorPos.x - 1, doorPos.y)))
				cells[doorPos.x - 1, doorPos.y] = CellState.groundCell;
			if (isValidPosition(roomProperties, new Vector2Int(doorPos.x, doorPos.y - 1)))
				cells[doorPos.x, doorPos.y - 1] = CellState.groundCell;
			if (isValidPosition(roomProperties, new Vector2Int(doorPos.x - 1, doorPos.y - 1)))
				cells[doorPos.x - 1, doorPos.y - 1] = CellState.groundCell;
		}
		return cells;
	}
	private static bool isValidPosition(RoomProperty roomProperties, Vector2Int position) {
		return (position.x >= 0 && position.y >= 0
			&& position.x <= roomProperties.basicDim.x - 1 && position.y <= roomProperties.basicDim.y - 1);
	}
	private static void placeTileGround(Vector3Int tilePos, Tile tile) {
		dungeonAsset.tilemapGround.SetTile(tilePos, tile);
	}
	private static void placeTileCollider(Vector3Int tilePos, Tile tile) {
		// Tile tile = new Tile();
		// tile.colliderType = Tile.ColliderType.Grid;
		// tile.color = Color.red;
		dungeonAsset.tilemapCollider.SetTile(tilePos, tile);
	}
	private static void placeTileWall(Vector3Int tilePos, Tile tile) {
		dungeonAsset.tilemapWall.SetTile(tilePos, tile);
	}
	private static void placeTileWallFront(Vector3Int tilePos, Tile tile) {
		dungeonAsset.tilemapWallFront.SetTile(tilePos, tile);
	}
	private static void placeDoor(Vector2Int position, Utils.GlobalDirection wallDirection) {
		//For now the door is only a hole 2x2
		placeTileGround(new Vector3Int(position.x, position.y, 0), dungeonAsset.groundTile);
		placeTileGround(new Vector3Int(position.x - 1, position.y, 0), dungeonAsset.groundTile);
		placeTileGround(new Vector3Int(position.x, position.y - 1, 0), dungeonAsset.groundTile);
		placeTileGround(new Vector3Int(position.x - 1, position.y - 1, 0), dungeonAsset.groundTile);
		placeTileWall(new Vector3Int(position.x, position.y, 0), null);
		placeTileWall(new Vector3Int(position.x - 1, position.y, 0), null);
		placeTileWall(new Vector3Int(position.x, position.y - 1, 0), null);
		placeTileWall(new Vector3Int(position.x - 1, position.y - 1, 0), null);
		placeTileWallFront(new Vector3Int(position.x, position.y, 0), null);
		placeTileWallFront(new Vector3Int(position.x - 1, position.y, 0), null);
		placeTileWallFront(new Vector3Int(position.x, position.y - 1, 0), null);
		placeTileWallFront(new Vector3Int(position.x - 1, position.y - 1, 0), null);
		//If is on the left wall
		if (wallDirection == Utils.GlobalDirection.WEST) {
			placeTileWall(new Vector3Int(position.x - 1, position.y + 1, 0), dungeonAsset.wallFront);
			placeTileWallFront(new Vector3Int(position.x - 1, position.y - 1, 0), dungeonAsset.wallTop);
		} else if (wallDirection == Utils.GlobalDirection.EST) {
			placeTileWall(new Vector3Int(position.x, position.y + 1, 0), dungeonAsset.wallFront);
			placeTileWallFront(new Vector3Int(position.x, position.y - 1, 0), dungeonAsset.wallTop);
		} else if (wallDirection == Utils.GlobalDirection.NORTH) {

		} else if (wallDirection == Utils.GlobalDirection.SOUTH) {
			placeTileWall(new Vector3Int(position.x - 1, position.y - 1, 0), dungeonAsset.wallLeft);
			placeTileWall(new Vector3Int(position.x, position.y - 1, 0), dungeonAsset.wallRight);
			placeTileWallFront(new Vector3Int(position.x - 1, position.y - 1, 0), null);
			placeTileWallFront(new Vector3Int(position.x, position.y - 1, 0), null);
		}
	}
}
