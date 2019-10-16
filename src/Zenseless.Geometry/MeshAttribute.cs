namespace Zenseless.Geometry
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Numerics;

	/// <summary>
	/// A class that encapsulates a single mesh attribute.
	/// </summary>
	public class MeshAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MeshAttribute"/> class. Use <seealso cref="Create{ELEMENT_TYPE}"/> instead.
		/// </summary>
		/// <param name="list">The list of elements.</param>
		/// <param name="baseType">The base type.</param>
		/// <param name="baseTypeCount">The base type count.</param>
		/// <param name="toArray">To array functor.</param>
		/// <exception cref="ArgumentNullException" />
		protected MeshAttribute(IList list, Type baseType, int baseTypeCount, Func<Array> toArray)
		{
			List = list ?? throw new ArgumentNullException(nameof(list));
			BaseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
			BaseTypeCount = baseTypeCount;
			this.toArray = toArray ?? throw new ArgumentNullException(nameof(toArray));
		}

		/// <summary>
		/// Creates an instance of a <seealso cref="MeshAttribute"/> class.
		/// </summary>
		/// <typeparam name="ELEMENT_TYPE">The element type.</typeparam>
		/// <returns></returns>
		public static MeshAttribute Create<ELEMENT_TYPE>()
		{
			var list = new List<ELEMENT_TYPE>();
			var baseType = typeof(ELEMENT_TYPE);
			var baseTypeCount = GetBaseTypeCount(baseType);
			if (1 < baseTypeCount)
			{
				baseType = typeof(float);
			}
			return new MeshAttribute(list, baseType, baseTypeCount, list.ToArray);
		}

		/// <summary>
		/// Gets the base type count. Element types used for attributes are made up of multiple base types like for instance Vector3 is made up of 3 floats. So for Vector3 the base type count is 3.
		/// </summary>
		/// <value>
		/// The base type count.
		/// </value>
		public int BaseTypeCount { get; }

		/// <summary>
		/// Copy the attribute elements into an array.
		/// </summary>
		/// <returns></returns>
		public Array ToArray() => toArray();

		/// <summary>
		/// Element types used for attributes are made up of multiple base types like for instance Vector3 is made up of 3 floats. So for Vector3 the base type is float.
		/// </summary>
		/// <value>
		/// The base type.
		/// </value>
		public Type BaseType { get; }

		/// <summary>
		/// Gets the internal list of elements.
		/// </summary>
		/// <typeparam name="ELEMENT_TYPE">The type of the element type.</typeparam>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		public List<ELEMENT_TYPE> GetList<ELEMENT_TYPE>()
		{
			var typedList = List as List<ELEMENT_TYPE>;
			if (typedList is null) throw new InvalidCastException($"Attribute is of type {List.GetType().FullName}.");
			return typedList;
		}

		/// <summary>
		/// Element types used for attributes are made up of multiple base types like for instance Vector3 is made up of 3 floats. So for Vector3 the base type count is 3.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The base type count.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static int GetBaseTypeCount(Type type)
		{
			if (mappingTypeToBaseTypeCount.TryGetValue(type, out int baseTypeCount))
			{
				return baseTypeCount;
			}
			throw new ArgumentException($"Unrecognized type '{type.FullName}' given. Use non generic version of function.");
		}

		private static readonly ReadOnlyDictionary<Type, int> mappingTypeToBaseTypeCount =
			new ReadOnlyDictionary<Type, int>(new Dictionary<Type, int>()
			{
				[typeof(float)] = 1,
				[typeof(byte)] = 1,
				[typeof(ushort)] = 1,
				[typeof(short)] = 1,
				[typeof(uint)] = 1,
				[typeof(int)] = 1,
				[typeof(float)] = 1,
				[typeof(Vector2)] = 2,
				[typeof(Vector3)] = 3,
				[typeof(Vector4)] = 4,
			});

		private readonly Func<Array> toArray;
		private IList List { get; }
	}
}
