namespace Tools.ObjectsPool
{
	public class PoolItemCallbacks
	{
		public System.Action OnAcquire { get; }
		public System.Action OnRelease { get; }

		public PoolItemCallbacks(System.Action OnAcquire, System.Action OnRelease)
		{
			this.OnAcquire = OnAcquire;
			this.OnRelease = OnRelease;
		}
	}
}