using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour 
{
	public GameObject[] 
		baseTileTypes,
		adjacentTileTypes;
	public GameObject[,] map;

	public void generateMap()
	{
		map = new GameObject[10,10];
		int i,j;
		for(i=0;i<10;i++)
			for(j=0;j<10;j++)
			{
				GameObject newTile = baseTileTypes[Random.Range(0,baseTileTypes.Length)];
				Quaternion tileRotation = Quaternion.Euler(new Vector3(0,Random.Range(0,3)*90,0));

				map[i,j] = (GameObject)Instantiate(newTile,
							new Vector3(i*10,0,j*10),
							tileRotation);

				map[i,j].transform.SetParent(gameObject.transform);
			}
			
	}
}
