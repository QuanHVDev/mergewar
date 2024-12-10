using System;
using System.Collections;
using UnityEngine;

public class ArrowWeapon : MonoBehaviour, IPoolable{
    public void Init(Vector3 startPoint, Vector3 endPoint, float height, float duration, Action onComplete) {
        StartCoroutine(MoveParabola(startPoint, endPoint, height, duration, onComplete));
    }
    
    IEnumerator MoveParabola(Vector3 startPoint, Vector3 endPoint, float height, float duration, Action onComplete) {
        float elapsedTime = 0;
        Vector3 vertex = GetVertex(startPoint, endPoint, height);
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Vector3 currentPosition = CalculateParabolaPoint(t, startPoint, vertex, endPoint);
            transform.position = currentPosition;
            yield return null;
        }

        onComplete?.Invoke();
        transform.position = endPoint;
        gameObject.Recycle();
    }
    
    public static Vector3 GetVertex(Vector3 start, Vector3 end, float h) {
        Vector3 midPoint = (start + end) / 2;
        return new Vector3(midPoint.x, midPoint.y + h, midPoint.z);
    }
    
    // Công thức nội suy tuyến tính để tính điểm trên đường parabol
    public static Vector3 CalculateParabolaPoint(float t, Vector3 start, Vector3 vertex, Vector3 end) {
        Vector3 l1 = Vector3.Lerp(start, vertex, t);
        Vector3 l2 = Vector3.Lerp(vertex, end, t);
        return Vector3.Lerp(l1, l2, t);
    }

    public void OnSpawnCallback() {
        
    }

    public void OnRecycleCallback() {
        
    }
}