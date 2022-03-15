using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

namespace ShipSimulator
{
    public interface IQuadTreeObject
    {
        float2 Position { get; set; }
    }

    public struct QuadTree<T> where T : IQuadTreeObject
    {
	    public bool IsCreated;
	    private int maxObjectCount;
	    private List<T> storedObjects;
	    private Rect bounds;
	    private QuadTree<T>[] cells;
	    private List<T> returnedObjects;
	    private List<T> cellObjects;

	    public QuadTree(int maxSize, Rect bounds)
	    {
		    this.bounds = bounds;
		    maxObjectCount = maxSize;
		    cells = new QuadTree<T>[4];
		    storedObjects = new List<T>(maxSize);
		    IsCreated = false;
		    returnedObjects = null;
		    cellObjects = null;
	    }

	    public void Insert(T objectToInsert)
	    {
		    if (cells[0].IsCreated)
		    {
			    int iCell = GetCellToInsertObject(objectToInsert.Position);
			    if (iCell > -1)
			    {
				    cells[iCell].Insert(objectToInsert);
			    }
			    return;
		    }

		    storedObjects.Add(objectToInsert);
		    //Objects exceed the maximum count
		    if (storedObjects.Count > maxObjectCount)
		    {
			    //Split the quad into 4 sections
			    if (!cells[0].IsCreated)
			    {
				    float subWidth = (bounds.width / 2f);
				    float subHeight = (bounds.height / 2f);
				    float x = bounds.x;
				    float y = bounds.y;
				    cells[0] = new QuadTree<T>(maxObjectCount, new Rect(x + subWidth, y, subWidth, subHeight));
				    cells[1] = new QuadTree<T>(maxObjectCount, new Rect(x, y, subWidth, subHeight));
				    cells[2] = new QuadTree<T>(maxObjectCount, new Rect(x, y + subHeight, subWidth, subHeight));
				    cells[3] = new QuadTree<T>(maxObjectCount, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
				    for (var j = 0; j < 4; j++)
					    cells[j].IsCreated = true;
			    }

			    //Reallocate this quads objects into its children
			    int i = storedObjects.Count - 1;
			    ;
			    while (i >= 0)
			    {
				    T storedObj = storedObjects[i];
				    int iCell = GetCellToInsertObject(storedObj.Position);
				    if (iCell > -1)
				    {
					    cells[iCell].Insert(storedObj);
				    }

				    storedObjects.RemoveAt(i);
				    i--;
			    }
		    }
	    }

	    public void Remove(T objectToRemove)
	    {
		    if (ContainsLocation(objectToRemove.Position))
		    {
			    storedObjects.Remove(objectToRemove);
			    if (cells[0].IsCreated)
			    {
				    for (int i = 0; i < 4; i++)
				    {
					    cells[i].Remove(objectToRemove);
				    }
			    }
		    }
	    }

	    public List<T> RetrieveObjectsInArea(Rect area)
	    {
		    if (rectOverlap(bounds, area))
		    {
			    List<T> returnedObjects = new List<T>();
			    for (int i = 0; i < storedObjects.Count; i++)
			    {
				    if (area.Contains(storedObjects[i].Position))
				    {
					    returnedObjects.Add(storedObjects[i]);
				    }
			    }

			    if (cells[0].IsCreated)
			    {
				    for (int i = 0; i < 4; i++)
				    {
					    List<T> cellObjects = cells[i].RetrieveObjectsInArea(area);
					    if (cellObjects != null)
					    {
						    returnedObjects.AddRange(cellObjects);
					    }
				    }
			    }

			    return returnedObjects;
		    }

		    return null;
	    }

	    // Clear quadtree
	    public void Clear()
	    {
		    storedObjects.Clear();

		    for (int i = 0; i < cells.Length; i++)
		    {
			    if (cells[i].IsCreated)
			    {
				    cells[i].Clear();
				    cells[i].IsCreated = false;
			    }
		    }
	    }

	    public bool ContainsLocation(Vector2 location)
	    {
		    return bounds.Contains(location);
	    }

	    private int GetCellToInsertObject(Vector2 location)
	    {
		    for (int i = 0; i < 4; i++)
		    {
			    if (cells[i].ContainsLocation(location))
			    {
				    return i;
			    }
		    }

		    return -1;
	    }

	    bool valueInRange(float value, float min, float max)
	    {
		    return (value >= min) && (value <= max);
	    }

	    bool rectOverlap(Rect A, Rect B)
	    {
		    bool xOverlap = valueInRange(A.x, B.x, B.x + B.width) ||
		                    valueInRange(B.x, A.x, A.x + A.width);

		    bool yOverlap = valueInRange(A.y, B.y, B.y + B.height) ||
		                    valueInRange(B.y, A.y, A.y + A.height);

		    return xOverlap && yOverlap;
	    }
	    
	    public void DrawDebug()
	    {
		    Gizmos.color = Color.green;
		    Gizmos.DrawLine(new Vector2(bounds.x, bounds.y),new Vector2(bounds.x,bounds.y+ bounds.height));
		    Gizmos.DrawLine(new Vector2(bounds.x, bounds.y),new Vector2(bounds.x+bounds.width,bounds.y));
		    Gizmos.DrawLine(new Vector2(bounds.x+bounds.width, bounds.y),new Vector2(bounds.x+bounds.width,bounds.y+ bounds.height));
		    Gizmos.DrawLine(new Vector2(bounds.x, bounds.y+bounds.height),new Vector2(bounds.x+bounds.width,bounds.y+bounds.height));
		    if(cells[0].IsCreated)
		    {			
			    for(int i  = 0; i < cells.Length; i++)
			    {
				    if(cells[i].IsCreated)
				    {
					    cells[i].DrawDebug();
				    }
			    }
		    }
	    }
    }
}