using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public static class TargetUtilities
{

    #region ROTATION METHODS

    public static void RotateTowardPosition(Transform myTransform, Vector2 targetPosition)
    {
        Quaternion targetRotation = GetRotationTowardTarget(targetPosition, myTransform.position);

        myTransform.rotation = targetRotation;
    }

    public static void RotateTowardPostion(Transform myTransform, Transform target)
    {
        Quaternion targetRotation = GetRotationTowardTarget(target, myTransform);
        myTransform.rotation = targetRotation;
    }

    public static void RotateSmoothlyTowardTarget(Transform target, Transform myTransform, float speed)
    {
        Quaternion targetRotation = GetRotationTowardTarget(target, myTransform);
        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, speed * Time.fixedDeltaTime);
    }

    public static void RotateSmoothlyTowardTarget(Vector3 targetPos, Transform myTransform, float speed)
    {
        Quaternion targetRotation = GetRotationTowardTarget(targetPos, myTransform.position);
        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, speed * Time.fixedDeltaTime);
    }

    //public static void RotateSmoothlyTowardTargetWithClamp(Vector3 targetPos, Transform myTransform, float speed, Vector2 clamp)
    //{
    //    Quaternion targetRotation = GetRotationTowardTargetWithClamp(targetPos, myTransform.position, clamp);
    //    myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, speed * Time.fixedDeltaTime);
    //}

    public static Quaternion GetRotationTowardTarget(Transform target, Transform myTransform)
    {
        return GetRotationTowardTarget(target.position, myTransform.position);
    }

    public static Quaternion GetRotationTowardTarget(Vector2 targetPosition, Vector2 myPosition)
    {
        Vector2 direction = targetPosition - myPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return targetRotation;
    }

    public static float GetRotationAngleTowardTarget(Vector2 targetPosition, Vector2 myPosition)
    {
        Vector2 direction = targetPosition - myPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        return angle;
    }

    public static Quaternion GetRotationTowardTarget2(Vector2 targetPosition, Vector2 myPosition)
    {
        float zRot = LookRotation(myPosition, targetPosition);

        return Quaternion.Euler(new Vector3(0f, 0f, zRot));
    }

    public static Quaternion GetRotationTowardTargetWithClamp(Vector2 targetPosition, Vector2 myPosition, Vector2 clamp)
    {
        Vector2 direction = targetPosition - myPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        Debug.Log(angle);

        float clampedAngle = Mathf.Clamp(angle, clamp.x, clamp.y);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, clampedAngle));

        return targetRotation;
    }

    public static Quaternion GetClampedRotation(Vector2 targetPosition, Transform myTransform, float speed, Vector2 clamp)
    {
        //float rotZ = LookRotation(myTransform.position, targetPosition);
        float rotZ = GetRotationAngleTowardTarget(targetPosition, myTransform.position);

        //Debug.Log(rotZ);

        if (rotZ < -180f)
            rotZ = -rotZ;

        float clampedZ = Mathf.Clamp(rotZ, clamp.x, clamp.y);

        //Vector3 to = new Vector3(0f, 0f, clampedZ);

        Quaternion toRotation = Quaternion.Euler(new Vector3(0f, 0f, clampedZ));

        return Quaternion.RotateTowards(myTransform.rotation, toRotation, speed * Time.deltaTime);

    }

    public static float LookRotation(Vector2 location, Vector2 target)
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(location.x, location.y) - new Vector3(target.x, target.y), Vector3.forward);
        rot.x = 0;
        rot.y = 0;
        return rot.eulerAngles.z;
    }

    public static float LookRotation2(Vector2 location, Vector2 target)
    {
        Quaternion rot = Quaternion.LookRotation(location - target, Vector3.forward);
        rot.x = 0;
        rot.y = 0;
        return rot.eulerAngles.z;
    }


    #endregion


    public static void RotateToNearestTarget(Collider2D initialTarget, Entity self, float radius, LayerMask mask, bool resetVelocity = true) {

        Entity nearest = FindNearestTarget(initialTarget, self.transform.position, radius, mask);

        if(nearest != null) {
            self.transform.rotation = GetRotationTowardTarget(nearest.transform, self.transform);

            if(resetVelocity == true) {
                self.Movement.MyBody.linearVelocity = Vector2.zero;
            }
        }

    }

    public static bool RotateToRandomNearbyTarget(Collider2D initialTarget, Entity self, float radius, LayerMask mask, bool resetVelocity = true) {
        Entity randomNearby = GetRandomNearbyEntity(initialTarget, self.transform.position, radius, mask);

        if(randomNearby != null) {
            self.transform.rotation = GetRotationTowardTarget(randomNearby.transform, self.transform);

            if (resetVelocity == true) {
                self.Movement.MyBody.linearVelocity = Vector2.zero;
            }

            return true;
        }


        return false;
    }

    public static Entity FindNearestTarget(Collider2D initialTarget, Vector2 myPosition, float radius, LayerMask mask) {
        List<Collider2D> nearbyColliders = Physics2D.OverlapCircleAll(myPosition, radius, mask).ToList();

        nearbyColliders.RemoveIfContains(initialTarget);

        Entity closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < nearbyColliders.Count; i++) {
            float distance = Vector2.Distance(myPosition, nearbyColliders[i].transform.position);
            if(distance < closestDistance) {
                closestDistance = distance;
                closest = nearbyColliders[i].GetComponent<Entity>();
            }
        }

        return closest;
    }

    public static Entity FindNearestTargetFromList(List<Entity> targets, Transform myTransform) {
        Entity closest = null;
        float closestDistance = float.MaxValue;

        for (int i = targets.Count -1; i >= 0 ; i--) {

            if (targets[i] == null) {
                targets.RemoveAt(i);
                continue;
            }
            
            float distance = Vector2.Distance(myTransform.position, targets[i].transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = targets[i];
            }
        }


        return closest;
    }

    public static Entity GetRandomNearbyEntity(Collider2D initialTarget, Vector2 myPosition, float radius, LayerMask mask) {
        List<Collider2D> nearbyColliders = Physics2D.OverlapCircleAll(myPosition, radius, mask).ToList();

        nearbyColliders.RemoveIfContains(initialTarget);

        if (nearbyColliders.Count == 0)
            return null;

        int randomindex = Random.Range(0, nearbyColliders.Count);

        return nearbyColliders[randomindex].GetComponent<Entity>();
    }

    public static Collider2D FindNearestTarget(Collider2D[] targets, Transform myTransform)
    {
        Collider2D result = null;

        Dictionary<Collider2D, float> distances = new Dictionary<Collider2D, float>();

        int count = targets.Length;
        for (int i = 0; i < count; i++)
        {
            float distance = Vector2.Distance(myTransform.position, targets[i].transform.position);
            distances.Add(targets[i], distance);
        }

        result = distances.OrderBy(d => d.Value).First().Key;

        return result;
    }

    public static Entity FindNearestTarget(List<Entity> targets, Transform myTransform) {
        Entity result = null;

        Dictionary<Entity, float> distances = new Dictionary<Entity, float>();

        int count = targets.Count;
        for (int i = 0; i < count; i++) {
            float distance = Vector2.Distance(myTransform.position, targets[i].transform.position);
            distances.Add(targets[i], distance);
        }

        result = distances.OrderBy(d => d.Value).First().Key;

        return result;
    }



    public static Vector2 CreateRandomDirection(float min, float max)
    {
        float xNoise = Random.Range(min, max);
        float yNoise = Random.Range(min, max);

        Vector2 result = new Vector2(
            Mathf.Sin(2 * Mathf.PI * xNoise / 360),
            Mathf.Sin(2 * Mathf.PI * yNoise / 360));

        return result;
    }


    public static Vector2 CreateKnockbackForce(Transform target, Transform myTransform, float forceMultiplier)
    {
        Vector2 direction = target.position - myTransform.position;
        Vector2 modifiedForce = direction.normalized * forceMultiplier;

        return modifiedForce;
    }

    public static Vector2 CreateKnockbackForceTowardPosition(Vector2 initialPosition, Vector2 goalPosition, float forceMultiplier)
    {
        Vector2 direction = goalPosition - initialPosition;

        Vector2 modifiedForce = direction.normalized * forceMultiplier;

        return modifiedForce;
    }

    public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float distance) {
        Vector3 P = distance * Vector3.Normalize(B - A) + A;
        return P;
    }

    public static Vector3 GetViewportToWorldPoint(float xMin = 0f, float yMin = 0f, float xMax = 1f, float yMax = 1f) {
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        Vector2 randomVector = new Vector2(randomX, randomY);

        Vector2 worldPoint = Camera.main.ViewportToWorldPoint(randomVector);

        return worldPoint;
    }


}
