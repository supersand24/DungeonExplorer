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

	//The vector a direction represents
	public static Vector2Int[] vectors = {
		new Vector2Int(0, 1),
		new Vector2Int(1, 0),
		new Vector2Int(0, -1),
		new Vector2Int(-1, 0)
	};

	//returns a vector2 of the given direction
	public static Vector2Int ToVector2Int(this MapDirection direction) {
		return vectors[(int) direction];
	}

	//returns a random direction
	public static MapDirection RandomValue {
		get {
			return (MapDirection)Random.Range(0, Count);
		}
	}

	//the opposites of the directions
	public static MapDirection[] opposites = {
		MapDirection.South,
		MapDirection.West,
		MapDirection.North,
		MapDirection.East
	};

	//returns the opposite of given direction
	public static MapDirection GetOpposite(this MapDirection direction) {
		return opposites[(int)direction];
	}

	//the math rotation of a direction
	private static Quaternion[] rotations = {
		Quaternion.identity, //North
		Quaternion.Euler(0f, 0f, 270f), //East
		Quaternion.Euler(0f, 0f, 180f), //South
		Quaternion.Euler(0f, 0f, 90f) //West
	};

	//returns the math rotation of the given direction
	public static Quaternion ToRotation(this MapDirection direction) {
		return rotations[(int)direction];
	}

}
