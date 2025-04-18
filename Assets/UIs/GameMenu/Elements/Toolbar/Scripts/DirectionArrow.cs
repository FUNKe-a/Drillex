using Godot;
using System;
using drillex.Assets.Entities.LayerManager;

public partial class DirectionArrow : TextureRect
{
    private LayerManager LayerManager;
    private Image _image;
    public override void _Ready()
    {
        _image = Texture.GetImage();
        LayerManager = (LayerManager)GetNode("/root/TestingStage/LayerManager");
        LayerManager.RotationChanged += OnRotationChanged;
            
        Rotation = 0;
    }
    private void OnRotationChanged(int rotationID)
    {
        Image temp = new Image();
        temp.CopyFrom(_image);
        for (int i = 0; i < rotationID; i++)
        {
            temp.Rotate90(ClockDirection.Clockwise);
        }

        ImageTexture rotatedTexture = ImageTexture.CreateFromImage(temp);
        Texture = rotatedTexture;
    }
}
