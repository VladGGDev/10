// This file was automatically generated, any modifications will be lost!
#pragma warning disable
namespace LDtkTypes;

using LDtk;

using Microsoft.Xna.Framework;

public class SawEntity : ILDtkEntity
{
    public string Identifier { get; set; }
    public System.Guid Iid { get; set; }
    public int Uid { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Vector2 Pivot { get; set; }
    public Rectangle Tile { get; set; }

    public Color SmartColor { get; set; }

    public float Speed { get; set; }
    public float WaitTime { get; set; }
    public bool IsLooping { get; set; }
    public Vector2[] Points { get; set; }
}
#pragma warning restore
