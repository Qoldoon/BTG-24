using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A decorative wall tile for use outside the play area.
/// Renders with the same sprite rules as the gameplay WallTiles RuleTile but
/// never spawns a GameObject, so it has zero runtime overhead.
///
/// Place this tile on the SAME tilemap as your gameplay wall tiles.
/// It recognises any RuleTile neighbour as a valid "wall" so sprites
/// blend seamlessly at the boundary with the real walls.
/// </summary>
[CreateAssetMenu(fileName = "BgWallTile", menuName = "Tiles/Background Wall Tile")]
public class BgWallTile : RuleTile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // Let the base RuleTile pick the correct sprite based on neighbours.
        base.GetTileData(position, tilemap, ref tileData);

        // Strip the GameObject so nothing is spawned for these decorative tiles.
        // tileData.gameObject = null;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            // Treat any RuleTile (including the gameplay WallTiles) as a valid wall neighbour.
            case TilingRule.Neighbor.This:
                return tile is RuleTile;
            case TilingRule.Neighbor.NotThis:
                return !(tile is RuleTile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}
