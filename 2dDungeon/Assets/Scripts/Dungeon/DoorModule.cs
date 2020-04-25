using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class DoorModule : MonoBehaviour {
	public enum DoorState {
		open, close
	};
	private DoorState doorState = DoorState.open;
	private Utils.Orientation orientation;
	private DungeonAssetModule dungeonAsset;
	private BoxCollider2D boxCollider;
	private SpriteRenderer spriteRenderer;
	private void Awake() {
		boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.enabled = false;
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = true;
	}
	public void setProperties(DungeonAssetModule dungeonAsset, Utils.Orientation orientation) {
		this.orientation = orientation;
		this.dungeonAsset = dungeonAsset;
		if (orientation == Utils.Orientation.horizontal) {
			spriteRenderer.sprite = dungeonAsset.spriteDoorFrontOpen;
			boxCollider.size = new Vector2(2, 0.2f);
		} else {
			spriteRenderer.sprite = dungeonAsset.spriteDoorSideOpen;
			boxCollider.size = new Vector2(0.2f, 2);
		}
	}
	public void openDoor() {
		boxCollider.enabled = false;
		if (orientation == Utils.Orientation.horizontal) {
			spriteRenderer.sprite = dungeonAsset.spriteDoorFrontOpen;
		} else {
			spriteRenderer.sprite = dungeonAsset.spriteDoorSideOpen;
		}
	}
	public void closeDoor() {
		boxCollider.enabled = true;
		if (orientation == Utils.Orientation.horizontal) {
			spriteRenderer.sprite = dungeonAsset.spriteDoorFrontClosed;
		} else {
			spriteRenderer.sprite = dungeonAsset.spriteDoorSideClosed;
		}
	}
}
