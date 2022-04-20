// Collider based perfect object placement system - https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503
// Copyright (c) 2022 fishdev https://assetstore.unity.com/publishers/36047

using UnityEngine;

namespace ObjectPlacerJobSystem
{
    /// <summary>
    /// http://answers.unity.com/comments/457779/view.html
    /// </summary>
    public class TextureChecker : MonoBehaviour
	{

		private static TerrainData mTerrainData;

		private static int alphamapWidth, alphamapHeight, mNumTextures;
		private static float[,,] mSplatmapData;

		void Awake()
		{
			mTerrainData = Terrain.activeTerrain.terrainData;
			alphamapWidth = mTerrainData.alphamapWidth;
			alphamapHeight = mTerrainData.alphamapHeight;

			mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
			mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);
		}

		private static Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos)
		{
			Vector3 vecRet = new Vector3();
			Terrain ter = Terrain.activeTerrain;
			Vector3 terPosition = ter.transform.position;
			vecRet.x = ((playerPos.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
			vecRet.z = ((playerPos.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
			return vecRet;
		}

		public static int GetActiveTerrainTextureIdx(Vector3 position)
		{
			Vector3 TerrainCord = ConvertToSplatMapCoordinate(position);
			int ret = 0;
			float comp = 0f;
			for (int i = 0; i < mNumTextures; i++)
			{
				if (comp < mSplatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
					ret = i;
			}
			return ret;
		}
	}
}
