using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    private const string ALLIED_TAG = "Allied", ENEMY_TAG = "Enemy";
    public static float getAngleDirection(Vector2 source, Vector2 target)
    {
        return Mathf.Atan2(target.y - source.y, target.x - source.x) * Mathf.Rad2Deg;
    }
    public static class Tag
    {
        public static bool isAllied(GameObject gameObject)
        {
            return string.Compare(ALLIED_TAG, gameObject.tag) == 0;
        }
        public static bool isEnemy(GameObject gameObject)
        {
            return string.Compare(ENEMY_TAG, gameObject.tag) == 0;
        }
        public static bool isAlliedOrEnemy(GameObject gameObject)
        {
            return string.Compare(ENEMY_TAG, gameObject.tag) == 0 ||
                string.Compare(ALLIED_TAG, gameObject.tag) == 0;
        }
        public static void setAllied(GameObject gameObject)
        {
            gameObject.tag = ALLIED_TAG;
        }
        public static void setEnemy(GameObject gameObject)
        {
            gameObject.tag = ENEMY_TAG;
        }
        public static bool isDifferetTags(GameObject obj1, GameObject obj2)
        {
            return string.Compare(obj1.tag, obj2.tag) != 0;
        }
        public static bool isOppositeSite(GameObject obj1, GameObject obj2)
        {
            return isAlliedOrEnemy(obj1) && isAlliedOrEnemy(obj2) && isDifferetTags(obj1, obj2);
        }
    }
    public static class Ai
    {
        public static bool isInRange(GameObject source, GameObject target, float range)
        {
            IUnitRadius sourceRadius = source.GetComponent<IUnitRadius>();
            IUnitRadius targetRadius = target.GetComponent<IUnitRadius>();
            if (sourceRadius != null && targetRadius != null)
                return (Vector2.Distance(source.transform.position, target.transform.position)
                    + sourceRadius.getUnitRadius() - targetRadius.getUnitRadius()) < range;
            return false;
        }
        public static bool isTargetVisible(GameObject source, GameObject target)
        {

            return isPathClearOfWall(source.transform.position, target.transform.position);
        }
    }
    public static bool isPathClearOfWall(Vector2 source, Vector2 destination)
    {
        LayerMask mask = LayerMask.GetMask(LayerMask.LayerToName(8));
        Vector2 direction = destination - source;
        float distance = Vector2.Distance(source, destination) + 1;
        RaycastHit2D hit = Physics2D.Raycast(source, direction, distance, mask);
        if (hit.collider != null)
        {
            // Debug.Log(hit.collider);
            return false;
        }
        return true;
    }
    public static Vector2 getRandomClearPointInRect(Rect rect, float clearRadius)
    {
        Vector2 point = new Vector2();
        do
        {
            point.x = Random.Range(rect.x, rect.x + rect.width);
            point.y = Random.Range(rect.y, rect.y + rect.height);
        } while (Physics2D.OverlapCircle(point, clearRadius) != null);

        return point;
    }
    public enum GlobalDirection
    {
        NORTH, EST, SOUTH, WEST
    };
    public enum Orientation
    {
        horizontal, vertical
    };
}