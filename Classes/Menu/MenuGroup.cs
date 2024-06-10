using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Menus;

public class MenuGroup
{
	struct MenuLink
	{
		public MenuElement Element;

		public MenuElement LeftElement;
		public MenuElement RightElement;
		public MenuElement UpElement;
		public MenuElement DownElement;
		public MenuElement InteractElement;

		public MenuLink(MenuElement element, MenuElement interact, MenuElement up, MenuElement right, MenuElement down, MenuElement left)
		{
			Element = element;
			InteractElement = interact;
			UpElement = up;
			RightElement = right;
			DownElement = down;
			LeftElement = left;
		}
		public MenuLink(MenuElement element, MenuElement interact) : this(element, interact, null, null, null, null) { }
		public MenuLink(MenuElement element) : this(element, null) { }
		public MenuLink(MenuLink copy) :
			this(copy.Element,
			copy.InteractElement,
			copy.UpElement,
			copy.RightElement,
			copy.DownElement,
			copy.LeftElement)
		{ }
	}


	public MenuElement SelectedElement 
	{
		get => _graph[_selected].Element;
		set => _selected = _graph[value].Element; 
	}


	MenuElement _selected;
	MenuLink SelectedLink
	{
		get => _graph[_selected];
		set => _selected = _graph[value.Element].Element;
	}
	Dictionary<MenuElement, MenuLink> _graph;


	// Construction
	public MenuGroup(MenuElement root)
	{
		_selected = root;
		_selected.IsSelected = true;
		_graph = new()
		{
			[root] = new MenuLink(root)
		};
	}


	public void AddNode(MenuElement element)
	{
		_graph[element] = new(element);
	}


	public void AddUpLink(MenuElement from, MenuElement to, bool bidirectional = true)
	{
		_graph[from] = new(_graph[from]) { UpElement = to };
		if (bidirectional)
			_graph[to] = new(_graph[to]) { DownElement = from };
	}

	public void AddDownLink(MenuElement from, MenuElement to, bool bidirectional = true)
	{
		_graph[from] = new(_graph[from]) { DownElement = to };
		if (bidirectional)
			_graph[to] = new(_graph[to]) { UpElement = from };
	}

	public void AddRightLink(MenuElement from, MenuElement to, bool bidirectional = true)
	{
		_graph[from] = new(_graph[from]) { RightElement = to };
		if (bidirectional)
			_graph[to] = new(_graph[to]) { LeftElement = from };
	}

	public void AddLeftLink(MenuElement from, MenuElement to, bool bidirectional = true)
	{
		_graph[from] = new(_graph[from]) { LeftElement = to };
		if (bidirectional)
			_graph[to] = new(_graph[to]) { RightElement = from };
	}

	public void AdInteractLink(MenuElement from, MenuElement to, bool bidirectional = true)
	{
		_graph[from] = new(_graph[from]) { InteractElement = to };
		if (bidirectional)
			_graph[to] = new(_graph[to]) { InteractElement = from };
	}



	// Logic
	public void Start(ContentManager content)
	{
		foreach (var element in _graph.Keys)
			element.Start(content);
	}

	public void Update()
	{
		HandleInput();
		foreach (var element in _graph.Keys)
			element.Update();
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		foreach (var element in _graph.Keys)
			element.Draw(spriteBatch);
	}



	// Input handling
	void HandleInput()
	{
		MenuElement prev = SelectedElement;

		if (Menu.GetAnyRightKeyDown() && SelectedLink.RightElement != null)
			SelectedElement = SelectedLink.RightElement;
		else if (Menu.GetAnyLeftKeyDown() && SelectedLink.LeftElement != null)
			SelectedElement = SelectedLink.LeftElement;
		else if(Menu.GetAnyUpKeyDown() && SelectedLink.UpElement != null)
			SelectedElement = SelectedLink.UpElement;
		else if(Menu.GetAnyDownKeyDown() && SelectedLink.DownElement != null)
			SelectedElement = SelectedLink.DownElement;
		else if(Menu.GetAnyInteractKeyDown() && SelectedLink.InteractElement != null)
			SelectedElement = SelectedLink.InteractElement;

		if (SelectedElement != prev)
		{
			prev.IsSelected = false;
			SelectedElement.IsSelected = true;
		}
	}
}
