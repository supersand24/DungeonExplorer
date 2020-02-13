using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {

	public Vector2Int coords;

	private MapCellEdge[] edges = new MapCellEdge[MapDirections.Count];
	private int initializedEdgeCount;

	//returns true if this cell has 4 cell edges
	public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == MapDirections.Count;
		}
	}

	//returns the cell edge of this cell in given direction
	public MapCellEdge GetEdge(MapDirection direction) {
		return edges[(int)direction];
	}

	//sets this cells edge in given direction to the given edge
	public void SetEdge(MapDirection direction, MapCellEdge edge) {
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}

}
