using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapDirection
{
	North,
	East,
	South,
	West
}

public static class MapDirections {

	public const int Count = 4;

	public static Vector2Int[] vectors = {
		new Vector2Int(0, 1),
		new Vector2Int(1, 0),
		new Vector2Int(0, -1),
		new Vector2Int(-1, 0)
	};

	public static Vector2Int ToVector2Int(this MapDirection direction) {
		return vectors[(int) direction];
	}

	public static MapDirection RandomValue {
		get {
			return (MapDirection)Random.Range(0, Count);
		}
	}

	public static MapDirection[] opposites = {
		MapDirection.South,
		MapDirection.West,
		MapDirection.North,
		MapDirection.East
	};

	public static MapDirection GetOpposite(this MapDirection direction) {
		return opposites[(int)direction];
	}

	private static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 90f)
	};

	public static Quaternion ToRotation(this MapDirection direction) {
		return rotations[(int)direction];
	}

}
