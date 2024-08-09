using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public abstract class Actor : ILDtkEntity
{
	public Collider Collider { get; set; }

	// LDtk
	public Vector2 Position { get; set; }
	public Vector2 Size { get; set; }
	public Vector2 Pivot { get; set; }

	public string Identifier { get; set; }
	public Guid Iid { get; set; }
	public int Uid { get; set; }
	public Rectangle Tile { get; set; }
	public Color SmartColor { get; set; }


	public abstract void Start(ContentManager content);
	public virtual void Update() { }
	public virtual void FixedUpdate() { }
	public abstract void Draw();
	public virtual void End() { }


	public void AddCollider(Collider collider)
	{
		Collider = collider;
		collider.Actor = this;
	}
}
