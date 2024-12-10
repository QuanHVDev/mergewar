// Decompiled with JetBrains decompiler
// Type: IPS.PoolExtensions
// Assembly: IPS.Core.Pooling, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3306DA4C-013D-4B35-BDA9-B4003306F513
// Assembly location: D:\Coding\Unity\UnityProject\Coloring\Assets\_IPS\Core\IPS.Core.Pooling.dll
// XML documentation location: D:\Coding\Unity\UnityProject\Coloring\Assets\_IPS\Core\IPS.Core.Pooling.xml

using UnityEngine;

public static class PoolExtensions{
    /// <summary>
    /// To check if gameobject has been registered with pool, no need to register again.
    /// </summary>
    public static bool IsRegistered(this GameObject prefab) =>
        SingletonBehaviour<PoolManager>.Instance.IsRegistered(prefab.gameObject);

    /// <summary>
    /// To check if gameobject can be spawn from pool, no need to instantiate new gameobject.
    /// </summary>
    public static bool HasPooled(this GameObject obj) => SingletonBehaviour<PoolManager>.Instance.HasPooled(obj);

    /// <summary>
    /// To check if gameobject has been spawned (currently active in scene)
    /// </summary>
    public static bool HasSpawned(this GameObject obj) => SingletonBehaviour<PoolManager>.Instance.HasSpawned(obj);

    /// <summary>
    /// Call this to register gameobject with Pool, so the game object will be add to pooled when recycle, else it will be destroy.
    /// <para>If your scrip inhenrit from interface "IPoolable", you no longer need to call this method.</para>
    /// </summary>
    public static void RegisterPool<T>(this T prefab) where T : Component =>
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab.gameObject, 0);

    /// <summary>
    /// Call this to register gameobject with Pool, so the game object will be add to pooled when recycle, else it will be destroy.
    /// <para>If your scrip inhenrit from interface "IPoolable", you no longer need to call this method.</para>
    /// </summary>
    public static void RegisterPool<T>(this T prefab, int initialPoolSize) where T : Component =>
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab.gameObject, initialPoolSize);

    /// <summary>
    /// Call this to register gameobject with Pool, so the game object will be add to pooled when recycle, else it will be destroy.
    /// <para>If your scrip inhenrit from interface "IPoolable", you no longer need to call this method.</para>
    /// </summary>
    public static void RegisterPool(this GameObject prefab) =>
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab, 0);

    /// <summary>
    /// Call this to register gameobject with Pool, so the game object will be add to pooled when recycle, else it will be destroy.
    /// <para>If your scrip inhenrit from interface "IPoolable", you no longer need to call this method.</para>
    /// </summary>
    public static void RegisterPool(this GameObject prefab, int initialPoolSize) =>
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab, initialPoolSize);

    /// <summary>
    /// Destroy gameobject which is register with pool (include pooled and spawned)
    /// </summary>
    public static void UnRegisterPool(this GameObject prefab) =>
        SingletonBehaviour<PoolManager>.Instance.UnRegisterPool(prefab);

    /// <summary>
    /// Destroy gameobject which is register with pool (include pooled and spawned)
    /// </summary>
    public static void UnRegisterPool<T>(this T prefab) where T : Component =>
        SingletonBehaviour<PoolManager>.Instance.UnRegisterPool(prefab.gameObject);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation)
        where T : Component, IPoolable {
        return SingletonBehaviour<PoolManager>.Instance.Spawn<T>(prefab, parent, position, scale, rotation);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Vector3 scale)
        where T : Component, IPoolable => prefab.Spawn<T>(parent, position, scale, Quaternion.identity);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component, IPoolable =>
        prefab.Spawn<T>(parent, position, Vector3.one, Quaternion.identity);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    public static T Spawn<T>(this T prefab, Transform parent) where T : Component, IPoolable =>
        prefab.Spawn<T>(parent, Vector3.zero, Vector3.one, Quaternion.identity);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    public static T Spawn<T>(this T prefab) where T : Component, IPoolable =>
        prefab.Spawn<T>((Transform)null, Vector3.zero, Vector3.one, Quaternion.identity);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Transform parent,
        Vector3 position,
        Quaternion rotation)
        where T : Component, IPoolable {
        return prefab.Spawn<T>(parent, position, Vector3.one, rotation);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Transform parent, Quaternion rotation) where T : Component, IPoolable =>
        prefab.Spawn<T>(parent, Vector3.zero, Vector3.one, rotation);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Vector3 position, Vector3 scale, Quaternion rotation)
        where T : Component, IPoolable => prefab.Spawn<T>((Transform)null, position, scale, rotation);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Vector3 position, Vector3 scale) where T : Component, IPoolable =>
        prefab.Spawn<T>((Transform)null, position, scale, Quaternion.identity);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component, IPoolable =>
        prefab.Spawn<T>((Transform)null, position, Vector3.one, rotation);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Vector3 position) where T : Component, IPoolable =>
        prefab.Spawn<T>((Transform)null, position, Vector3.one, Quaternion.identity);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Quaternion rotation) where T : Component, IPoolable =>
        prefab.Spawn<T>((Transform)null, Vector3.zero, Vector3.one, rotation);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        bool registerPoolIfNeed = true)
        where T : Component {
        return SingletonBehaviour<PoolManager>.Instance.Spawn<T>(prefab, parent, position, scale, rotation,
            registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        bool registerPoolIfNeed = true)
        where T : Component {
        return prefab.Spawn<T>(parent, position, scale, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    public static T Spawn<T>(
        this T prefab,
        Transform parent,
        Vector3 position,
        bool registerPoolIfNeed = true)
        where T : Component {
        return prefab.Spawn<T>(parent, position, Vector3.one, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    public static T Spawn<T>(this T prefab, Transform parent, bool registerPoolIfNeed = true) where T : Component =>
        prefab.Spawn<T>(parent, Vector3.zero, Vector3.one, Quaternion.identity, registerPoolIfNeed);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    public static T Spawn<T>(this T prefab, bool registerPoolIfNeed = true) where T : Component =>
        prefab.Spawn<T>((Transform)null, Vector3.zero, Vector3.one, Quaternion.identity, registerPoolIfNeed);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Transform parent,
        Vector3 position,
        Quaternion rotation,
        bool registerPoolIfNeed = true)
        where T : Component {
        return prefab.Spawn<T>(parent, position, Vector3.one, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    public static T Spawn<T>(
        this T prefab,
        Transform parent,
        Quaternion rotation,
        bool registerPoolIfNeed = true)
        where T : Component {
        return prefab.Spawn<T>(parent, Vector3.zero, Vector3.one, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        bool registerPoolIfNeed = true)
        where T : Component {
        return prefab.Spawn<T>((Transform)null, position, scale, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Vector3 position,
        Vector3 scale,
        bool registerPoolIfNeed = true)
        where T : Component {
        return prefab.Spawn<T>((Transform)null, position, scale, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(
        this T prefab,
        Vector3 position,
        Quaternion rotation,
        bool registerPoolIfNeed = true)
        where T : Component {
        return prefab.Spawn<T>((Transform)null, position, Vector3.one, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Vector3 position, bool registerPoolIfNeed = true) where T : Component =>
        prefab.Spawn<T>((Transform)null, position, Vector3.one, Quaternion.identity, registerPoolIfNeed);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static T Spawn<T>(this T prefab, Quaternion rotation, bool registerPoolIfNeed = true) where T : Component =>
        prefab.Spawn<T>((Transform)null, Vector3.zero, Vector3.one, rotation, registerPoolIfNeed);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        bool registerPoolIfNeed = true) {
        return SingletonBehaviour<PoolManager>.Instance.Spawn(prefab, parent, position, scale, rotation,
            registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn(parent, position, scale, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    public static GameObject Spawn(
        this GameObject prefab,
        Transform parent,
        Vector3 position,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn(parent, position, Vector3.one, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    public static GameObject Spawn(
        this GameObject prefab,
        Transform parent,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn(parent, Vector3.zero, Vector3.one, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    public static GameObject Spawn(this GameObject prefab, bool registerPoolIfNeed = true) =>
        prefab.Spawn((Transform)null, Vector3.zero, Vector3.one, Quaternion.identity, registerPoolIfNeed);

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Transform parent,
        Vector3 position,
        Quaternion rotation,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn(parent, position, Vector3.one, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="parent">default value = null</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    public static GameObject Spawn(
        this GameObject prefab,
        Transform parent,
        Quaternion rotation,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn(parent, Vector3.zero, Vector3.one, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn((Transform)null, position, scale, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="scale">defaul value = Vector3.one</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Vector3 position,
        Vector3 scale,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn((Transform)null, position, scale, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Vector3 position,
        Quaternion rotation,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn((Transform)null, position, Vector3.one, rotation, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="position">defaul value = Vector3.zero</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Vector3 position,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn((Transform)null, position, Vector3.one, Quaternion.identity, registerPoolIfNeed);
    }

    /// <summary>
    /// Instantiate (clone) a game object from "prefab" object.
    /// <para>Source object is "IPoolable"</para>
    /// <para>If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool" before, output gameObject will be add into pooled list and can be reuse when recycle.</para>
    /// </summary>
    /// <param name="rotation">default value = Quaternion.identity</param>
    /// <returns></returns>
    public static GameObject Spawn(
        this GameObject prefab,
        Quaternion rotation,
        bool registerPoolIfNeed = true) {
        return prefab.Spawn((Transform)null, Vector3.zero, Vector3.one, rotation, registerPoolIfNeed);
    }

    /// <summary>
    ///  Remove game object from the scene (Disable or Destroy)
    /// <para>If game object is "IPoolable" (or was register by "RegisterPool"), it will be set to DISABLE, else it will be DESTROY </para>
    ///  </summary>
    public static void Recycle<T>(this T obj) where T : Component =>
        SingletonBehaviour<PoolManager>.Instance.Recycle<T>(obj);

    /// <summary>
    ///  Remove game object from the scene (Disable or Destroy)
    /// <para>If game object is "IPoolable" (or was register by "RegisterPool"), it will be set to DISABLE, else it will be DESTROY </para>
    ///  </summary>
    public static void Recycle(this GameObject obj) => SingletonBehaviour<PoolManager>.Instance.Recycle(obj);

    /// <summary>
    ///  Remove game object from the scene (Disable or Destroy)
    /// <para>If game object is "IPoolable" (or was register by "RegisterPool"), it will be set to DISABLE, else it will be DESTROY </para>
    ///  </summary>
    public static void RecycleAll<T>(this T prefab) where T : Component =>
        SingletonBehaviour<PoolManager>.Instance.RecycleAll<T>(prefab);

    /// <summary>
    ///  Remove game object from the scene (Disable or Destroy)
    /// <para>If game object is "IPoolable" (or was register by "RegisterPool"), it will be set to DISABLE, else it will be DESTROY </para>
    ///  </summary>
    public static void RecycleAll(this GameObject prefab) =>
        SingletonBehaviour<PoolManager>.Instance.RecycleAll(prefab);
}