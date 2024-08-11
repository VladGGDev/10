using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using LDtk;
using LDtk.Renderer;
using System;
using System.ComponentModel;

public abstract class Scene
{
	protected List<Actor> Actors { get; set; } = new List<Actor>();
	List<Actor> _queuedRemovedActors = new List<Actor>();
	List<Actor> _queuedAddedActors = new List<Actor>();
	public LDtkLevel Level { get; set; }
	public Camera Camera { get; set; }

	// Events
	public event EventHandler<CollectionChangeEventArgs> ActorsListChanged;
	bool _didFirstUpdate = false;
	public event EventHandler FirstUpdate;
	public event EventHandler SceneEnded;

	public Scene(LDtkLevel level)
	{
		Level = level;
		Camera = new(Vector2.Zero, Main.TargetScreenHeight * 1f);
	}


	public virtual void Start(ContentManager content)
	{
		HandleActorQueues();
		foreach (var actor in Actors)
			actor.Start(content);
	}

	public virtual void Update()
	{
		OnFirstUpdate();

		foreach (var actor in Actors)
			actor.Update();
		HandleActorQueues();
	}

	public virtual void FixedUpdate()
	{
		foreach (var actor in Actors)
			actor.FixedUpdate();
		HandleActorQueues();
	}

	public virtual void Draw(ExampleRenderer levelRenderer)
	{
		// Draw the level
		levelRenderer?.RenderPrerenderedLevel(Level);

		foreach (var actor in Actors)
			actor.Draw();
	}

	public virtual void End()
	{
		Collider.Colliders.Clear();
		foreach (var actor in Actors)
			actor.End();

		OnSceneEnded();
	}



	protected virtual void OnFirstUpdate()
	{
		if (!_didFirstUpdate)
		{
			FirstUpdate?.Invoke(this, EventArgs.Empty);
			_didFirstUpdate = true;
		}
	}

	protected virtual void OnSceneEnded()
	{
		SceneEnded?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnActorListChanged(CollectionChangeAction eventArgs, Actor actor)
	{
		ActorsListChanged?.Invoke(this, new CollectionChangeEventArgs(eventArgs, actor));
	}





	public void AddActor(Actor actor)
	{
		_queuedAddedActors.Add(actor);
		OnActorListChanged(CollectionChangeAction.Add, actor);
	}

	public void AddActorRange(IEnumerable<Actor> actors)
	{
		_queuedAddedActors.AddRange(actors);

		if (ActorsListChanged != null)
			foreach (var actor in actors)
				OnActorListChanged(CollectionChangeAction.Add, actor);
	}

	public void RemoveActor(Actor actor)
	{
		_queuedRemovedActors.Add(actor);
		OnActorListChanged(CollectionChangeAction.Remove, actor);
	}

	public void RemoveActorRange(IEnumerable<Actor> actors)
	{
		_queuedRemovedActors.AddRange(actors);

		if (ActorsListChanged != null)
			foreach (var actor in actors)
				OnActorListChanged(CollectionChangeAction.Remove, actor);
	}

	public ActorT GetActor<ActorT>() where ActorT : Actor
	{
		return Actors.Find((Actor actor) => actor is ActorT) as ActorT;
	}

	// predicate can also be a delegate of type Predicate<ActorT>
	public ActorT GetActor<ActorT>(Func<ActorT, bool> predicate) where ActorT : Actor
	{
		return Actors.Find((Actor actor) => actor is ActorT && predicate(actor as ActorT)) as ActorT;
	}

	public ActorT[] GetAllActors<ActorT>() where ActorT : Actor
	{
		return (Actors.FindAll((Actor actor) => actor is ActorT) as List<ActorT>).ToArray();
	}

	public ActorT[] GetAllActors<ActorT>(Func<ActorT, bool> predicate) where ActorT : Actor
	{
		return (Actors.FindAll((Actor actor) => actor is ActorT && predicate(actor as ActorT)) as List<ActorT>).ToArray();
	}

	void HandleActorQueues()
	{
		// Remove actors
		foreach (var actor in _queuedRemovedActors)
		{
			Actors.Remove(actor);
			Collider.Colliders.Remove(actor?.Collider);
		}
		_queuedRemovedActors.Clear();

		// Add actors
		foreach (var actor in _queuedAddedActors)
		{
			Actors.Add(actor);
			actor.Position += Main.EntityLayerOffset;
		}
		_queuedAddedActors.Clear();
	}
}
