using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
[CreateAssetMenu(fileName = "RoomProperty", menuName = "ScriptableObjects/RoomProperty")]
public class RoomProperty : ScriptableObject {
	public enum RoomCategory {
		spawnRoom, enemyRoom, bossRoom, interconnectionRoom
	};
	[SerializeField] public Vector2Int basicDim;
	[SerializeField] public List<Vector2Int> doorsPosition;
	public List<RectInt> removedGround;
	public RoomCategory category;

	// [Button]
	// public bool IsValid()
	// {
	//     if (basicDim.x == 0 || basicDim.y == 0)
	//     {
	//         Debug.LogWarning("Dimensions not correct");
	//         return false;
	//     }
	//     for (int ind = 0; ind < doorsPosition.Count; ind++)
	//     {
	//         if (doorsPosition[ind].x != 0 && doorsPosition[ind].y != 0)
	//         {
	//             Debug.LogWarning("The doors must to be on the outer wall");
	//             return false;
	//         }
	//         if (doorsPosition[ind].x < 1 || doorsPosition[ind].x >= basicDim.x
	//             || doorsPosition[ind].y < 1 || doorsPosition[ind].y >= basicDim.y)
	//         {
	//             Debug.LogWarning("Door position is not correct");
	//             return false;
	//         }
	//     }
	//     Debug.Log("Room correct");
	//     return true;
	// }
}
