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


	public MenuElement Selected
	{
		get => _selectedLink.Element;
		set => _selectedLink = _graph[value];
	}
	MenuLink _selectedLink;
	Dictionary<MenuElement, MenuLink> _graph;


	// Construction
	public MenuGroup(MenuElement root)
	{
		_selectedLink.Element = root;
		_selectedLink.Element.IsSelected = true;
		_graph = new()
		{
			[root] = _selectedLink
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
		MenuElement prev = _selectedLink.Element;

		if (Menu.GetAnyRightKeyDown() && _selectedLink.RightElement != null)
			_selectedLink.Element = _selectedLink.RightElement;
		else if (Menu.GetAnyLeftKeyDown() && _selectedLink.LeftElement != null)
			_selectedLink.Element = _selectedLink.LeftElement;
		else if(Menu.GetAnyUpKeyDown() && _selectedLink.UpElement != null)
			_selectedLink.Element = _selectedLink.UpElement;
		else if(Menu.GetAnyDownKeyDown() && _selectedLink.DownElement != null)
			_selectedLink.Element = _selectedLink.DownElement;
		else if(Menu.GetAnyInteractKeyDown() && _selectedLink.InteractElement != null)
			_selectedLink.Element = _selectedLink.InteractElement;

		if (_selectedLink.Element != prev)
		{
			prev.IsSelected = false;
			_selectedLink.Element.IsSelected = true;
		}
	}
}
