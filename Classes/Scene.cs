using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using LDtk;
using LDtk.Renderer;

public abstract class Scene
{
	List<Actor> _actors;
	List<Actor> _queuedRemovedActors = new List<Actor>();
	List<Actor> _queuedAddedActors = new List<Actor>();
	public LDtkLevel Level { get; set; }
	public Camera Camera { get; set; }

	public Scene(LDtkLevel level)
	{
		Level = level;
		Camera = new(Vector2.Zero, Main.TargetScreenHeight * 1f);
		_actors = new List<Actor>();
	}


	public virtual void Start(ContentManager content)
	{
		HandleActorQueues();
		foreach (var actor in _actors)
			actor.Start(content);
	}

	public virtual void Update()
	{
		foreach (var actor in _actors)
			actor.Update();
		HandleActorQueues();
	}

	public virtual void FixedUpdate()
	{
		foreach (var actor in _actors)
			actor.FixedUpdate();
		HandleActorQueues();
	}

	public virtual void Draw(SpriteBatch spriteBatch, ExampleRenderer levelRenderer)
	{
		// Draw the level
		if (Level != null)
			levelRenderer.RenderPrerenderedLevel(Level);

		foreach (var actor in _actors)
			actor.Draw(spriteBatch);
	}

	public virtual void End()
	{
		Collider.Colliders.Clear();
		foreach (var actor in _actors)
			actor.End();
	}





	public void AddActor(Actor actor)
	{
		_queuedAddedActors.Add(actor);
	}

	public void AddActorRange(IEnumerable<Actor> actors)
	{
		_queuedAddedActors.AddRange(actors);
	}

	public void RemoveActor(Actor actor)
	{
		_queuedRemovedActors.Add(actor);
	}

	public void RemoveActorRange(IEnumerable<Actor> actors)
	{
		_queuedRemovedActors.AddRange(actors);
	}

	public ActorType GetActor<ActorType>() where ActorType : Actor
	{
		return _actors.Find((Actor actor) => actor is ActorType) as ActorType;
	}

	//public Actor GetActor<ActorType>(Func<Actor, bool> predicate)
	//{
	//	return _actors.Find((Actor actor) => actor is ActorType && predicate(actor));
	//}

	public ActorType[] GetAllActors<ActorType>() where ActorType : Actor
	{
		return (_actors.FindAll((Actor actor) => actor is ActorType) as List<ActorType>).ToArray();
	}

	//public Actor[] GetAllActors<ActorType>(Func<Actor, bool> predicate)
	//{
	//	return _actors.FindAll((Actor actor) => actor is ActorType && predicate(actor)).ToArray();
	//}

	void HandleActorQueues()
	{
		// Remove actors
		foreach (var actor in _queuedRemovedActors)
		{
			_actors.Remove(actor);
			Collider.Colliders.Remove(actor?.Collider);
		}
		_queuedRemovedActors.Clear();

		// Add actors
		foreach (var actor in _queuedAddedActors)
		{
			_actors.Add(actor);
		}
		_queuedAddedActors.Clear();
	}
}
