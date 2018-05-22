using Zenseless.Patterns;
using OpenTK.Graphics.OpenGL4;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Patterns.Disposable" />
	public class QueryObject : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="QueryObject"/> class.
		/// </summary>
		public QueryObject()
		{
			GL.GenQueries(1, out id);
		}

		/// <summary>
		/// Activates the specified target.
		/// </summary>
		/// <param name="target">The target.</param>
		public void Activate(QueryTarget target)
		{
			Target = target;
			GL.BeginQuery(target, id);
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.EndQuery(Target);
		}

		/// <summary>
		/// Gets a value indicating whether this instance is finished.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is finished; otherwise, <c>false</c>.
		/// </value>
		public bool IsFinished
		{
			get
			{
				int isFinished;
				GL.GetQueryObject(id, GetQueryObjectParam.QueryResultAvailable, out isFinished);
				return 1 == isFinished;
			}
		}

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>
		/// The result.
		/// </value>
		public int Result
		{
			get
			{
				int result;
				GL.GetQueryObject(id, GetQueryObjectParam.QueryResult, out result);
				return result;
			}
		}

		/// <summary>
		/// Gets the result long.
		/// </summary>
		/// <value>
		/// The result long.
		/// </value>
		public long ResultLong
		{
			get
			{
				long result;
				GL.GetQueryObject(id, GetQueryObjectParam.QueryResult, out result);
				return result;
			}
		}

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		public QueryTarget Target { get; private set; }

		/// <summary>
		/// Tries the get result.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <returns></returns>
		public bool TryGetResult(out int result)
		{
			result = -1;
			GL.GetQueryObject(id, GetQueryObjectParam.QueryResultNoWait, out result);
			return -1 != result;
		}

		/// <summary>
		/// Tries the get result.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <returns></returns>
		public bool TryGetResult(out long result)
		{
			result = -1;
			GL.GetQueryObject(id, GetQueryObjectParam.QueryResultNoWait, out result);
			return -1 != result;
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			GL.DeleteQueries(1, ref id);
		}

		/// <summary>
		/// The identifier
		/// </summary>
		private int id;
	}
}
