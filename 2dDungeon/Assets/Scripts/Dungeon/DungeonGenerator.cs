using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;
using System;
public static class DungeonGenerator {
	public class DGsettings {

	}
	[Serializable]
	public class DGresult {
		[Serializable]
		public class Room {
			public Vector2Int position;
			public RoomProperty properties;
			public Utils.GlobalDirection rotation;
			public List<bool> doorActive;
		}
		public List<Room> rooms;
	}
	public static DGresult Generate(DGsettings settings) {
		return null;
	}
}