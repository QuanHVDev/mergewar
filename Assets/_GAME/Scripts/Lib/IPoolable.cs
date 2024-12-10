/// <summary>
/// inherit this interface to mark the game object is poolable, it mean the gameObject will be add to pooled when recycle, else it will be destroy.
/// </summary>
public interface IPoolable
{
    /// This will be call automaticaly by "PoolManager.Spawn". Do not call this outside.
    /// <para>Init your variable inside here.</para>
    /// </summary>
    void OnSpawnCallback();

    /// <summary>
    /// This will be call automaticaly by "PoolManager.Recycle". Do not call this outside.
    /// <para>Put your callback action inside here. This call before OnDisable</para>
    /// </summary>
    void OnRecycleCallback();
}