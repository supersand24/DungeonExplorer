using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapCellEdge : MonoBehaviour {
    
	public MapCell cell, otherCell;
	public MapDirection direction;

	public void Initialize(MapCell cell, MapCell otherCell, MapDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector2.zero;
		transform.localRotation = direction.ToRotation();
	}

}
