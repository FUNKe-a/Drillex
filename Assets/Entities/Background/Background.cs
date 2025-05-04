using Godot;
using System.Linq;
using drillex.Common.Scripts;

public partial class Background : TileMapLayer
{
    public BackgroundTile[,] CreateBackgroundMatrix(int xBoundary, int yBoundary)
    {
        BackgroundTile[,] backgroundMatrix = new BackgroundTile[xBoundary, yBoundary];

        for (int i = 0; i < xBoundary; i++)
            for (int j = 0; j < yBoundary; j++)
                backgroundMatrix[i, j] = new BackgroundTile();
        
        for (int i = 0; i < xBoundary; i++)
        {
            for (int j = 0; j < yBoundary; j++)
            {
                TileData data = GetCellTileData(new Vector2I(i, j));
                if (data is null)
                {
                    backgroundMatrix[i, j].IsMineable = false;
                }
                else if (data.HasCustomData("Mineable"))
                {
                    backgroundMatrix[i, j].IsMineable =
                        GetCellTileData(new Vector2I(i, j)).GetCustomData("Mineable").AsBool();
                }
            }
        }
        
        return backgroundMatrix;
    }
}
