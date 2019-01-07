using System;
using System.Collections.Generic;

namespace MiniGalaxyBirds
{
	public interface IComponent
	{
		//IComponentContainer Container { get; set; }
	}
	
	public interface IComponentContainer 
	{
		IEnumerable<IComponent> Components { get; }

		TYPE GetComponent<TYPE>() where TYPE : class;
	}

	sealed class ComponentRegistry
	{
		//sealed private class Component : IComponent
		//{
		//}

		sealed private class ComponentContainer : IComponentContainer
		{
			public void Add(IComponent component)
			{
				components.Add(component);
				foreach (Type type in GetDerivedTypes(component))
				{
					typeComponentInstance[type] = component;
				}
			}

			public void ClearComponents()
			{
				components.Clear();
				typeComponentInstance.Clear();
			}

			public TYPE GetComponent<TYPE>() where TYPE : class
			{
				if (typeComponentInstance.TryGetValue(typeof(TYPE), out IComponent component))
				{
					return component as TYPE;
				}
				else
				{
					return null;
				}
			}

			public static IEnumerable<Type> GetDerivedTypes(IComponent component)
			{
				yield return component.GetType();
				//add implemented interfaces
				foreach (Type iType in component.GetType().GetInterfaces())
				{
					if (typeof(IComponent) != iType)
					{
						yield return iType;
					}
				}
				//add base classes
				Type type = component.GetType().BaseType;
				while (typeof(object) != type)
				{
					yield return type;
					type = type.BaseType;
				}
			}

			public IEnumerable<IComponent> Components { get { return components; } }

			private readonly HashSet<IComponent> components = new HashSet<IComponent>();
			private readonly Dictionary<Type, IComponent> typeComponentInstance = new Dictionary<Type, IComponent>();
		}

		public IComponentContainer CreateComponentContainer()
		{
			ComponentContainer container = new ComponentContainer();
			containers.Add(container);
			return container;
		}

		public void RegisterComponentTo(IComponentContainer cont, IComponent component)
		{
			if (component is null || cont is null) return;
			ComponentContainer container = (ComponentContainer)cont;
			container.Add(component);
			component2Owner.Add(component, container);
			foreach (Type type in ComponentContainer.GetDerivedTypes(component))
			{
				Add(type);
				componentType2ContainerInstances[type].Add(container);
				componentType2ComponentInstances[type].Add(component);
			}
		}

		public void Unregister(IComponentContainer cont)
		{
			ComponentContainer container = (ComponentContainer)cont;
			foreach (IComponent component in container.Components)
			{
				component2Owner.Remove(component);
				foreach (Type type in ComponentContainer.GetDerivedTypes(component))
				{
					componentType2ComponentInstances[type].Remove(component);
					componentType2ContainerInstances[type].Remove(container);
				}
			}
			container.ClearComponents();
			containers.Remove(cont);
		}

		public class ComponentEnumerator<TYPE> : IEnumerable<TYPE>, IEnumerator<TYPE> where TYPE: class
		{
			private IEnumerator<IComponent> enumerator;

			public ComponentEnumerator(IEnumerable<IComponent> components)
			{
				enumerator = components.GetEnumerator();
			}

			public IEnumerator<TYPE> GetEnumerator()
			{
				return this;
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this;

			public TYPE Current
			{
				get { return enumerator.Current as TYPE; }
			}

			public void Dispose()
			{
				enumerator.Dispose();
			}

			object System.Collections.IEnumerator.Current
			{
				get { return enumerator.Current; }
			}

			public bool MoveNext()
			{
				return enumerator.MoveNext();
			}

			public void Reset()
			{
				enumerator.Reset();
			}
		}

		public IEnumerable<COMPONENT_TYPE> GetComponents<COMPONENT_TYPE>() where COMPONENT_TYPE : class
		{
			Add(typeof(COMPONENT_TYPE));
			var a = componentType2ComponentInstances[typeof(COMPONENT_TYPE)];
			return new ComponentEnumerator<COMPONENT_TYPE>(a);
		}

		public IComponentContainer GetContainer(IComponent component)
		{
			return component2Owner[component];
		}

		public IEnumerable<IComponentContainer> GetAllContainer()
		{
			return containers;
		}
		public IEnumerable<IComponentContainer> GetAllContainerWithComponent<TYPE>()
		{
			Add(typeof(TYPE));
			return componentType2ContainerInstances[typeof(TYPE)];
		}

		private List<IComponentContainer> containers = new List<IComponentContainer>();
		private Dictionary<Type, HashSet<ComponentContainer>> componentType2ContainerInstances = new Dictionary<Type, HashSet<ComponentContainer>>();
		private Dictionary<Type, HashSet<IComponent>> componentType2ComponentInstances = new Dictionary<Type, HashSet<IComponent>>();
		private Dictionary<IComponent, ComponentContainer> component2Owner = new Dictionary<IComponent, ComponentContainer>();

		private void Add(Type type)
		{
			if (!componentType2ComponentInstances.ContainsKey(type))
			{
				componentType2ContainerInstances.Add(type, new HashSet<ComponentContainer>());
				componentType2ComponentInstances.Add(type, new HashSet<IComponent>());
			}
		}
	}
}
