using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {

	public Vector2Int coords;

	private MapCellEdge[] edges = new MapCellEdge[MapDirections.Count];
	private int initializedEdgeCount;

	public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == MapDirections.Count;
		}
	}

	public MapCellEdge GetEdge(MapDirection direction) {
		return edges[(int)direction];
	}

	public void SetEdge(MapDirection direction, MapCellEdge edge) {
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}

	public MapDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range(0, MapDirections.Count - initializedEdgeCount);
			for (int i=0; i<MapDirections.Count; i++) {
				if (edges[i] == null) {
					if (skips == 0) {
						return (MapDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MapCell has no uninitialized directions left");
		}
	}

}
