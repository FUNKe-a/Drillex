using Godot;
using System;
using drillex.Assets.Entities.LayerManager;

public partial class DirectionArrow : TextureRect
{
    public LayerManager LayerManager {get; set;}
    private Image _image;
    public override async void _Ready()
    {
        await ToSignal(Owner.Owner, "ready");
        _image = Texture.GetImage();
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
