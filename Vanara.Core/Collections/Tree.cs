using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanara.Collections
{
	/// <summary>
	/// A hierarchical tree containing nodes of type <typeparamref name="TValue"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of the node.</typeparam>
	public class Tree<TValue> : SortedDictionary<TValue, Tree<TValue>>
	{
		/// <summary>
		/// Creates a tree from a list of paired values.
		/// </summary>
		/// <param name="input">A list of paired values.</param>
		/// <returns>A tree of leafs constructed from the list.</returns>
		public static Tree<TValue> FromTupleList(List<Tuple<TValue, TValue>> input)
		{
			var ret = new Tree<TValue>();
			var groups = input.GroupBy(i => i.Item2);
			foreach (var item in groups.Where(g => !input.Exists(i => Equals(i.Item1, g.Key))))
				ret.Add(item, groups);
			return ret;
		}

		private void Add(IGrouping<TValue, Tuple<TValue, TValue>> item, IEnumerable<IGrouping<TValue, Tuple<TValue, TValue>>> groups)
		{
			var children = new Tree<TValue>();
			Add(item.Key, children);
			foreach (var child in groups.Where(g => Equals(g.Key, item.Key)))
				children.Add(child, groups);
		}
	}
}