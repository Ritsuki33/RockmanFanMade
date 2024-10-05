using UnityEngine;

public static class ExtendTransform
{
    public static Vector2 position_xy(this Transform transform) => new Vector2(transform.position.x, transform.position.y);
    public static Vector2 position_xz(this Transform transform) => new Vector2(transform.position.x, transform.position.z);
    public static Vector2 position_yz(this Transform transform) => new Vector2(transform.position.x, transform.position.z);

    public static void position_xy(this Transform transform,Vector2 val) => transform.position = new Vector3(val.x, val.y, transform.position.z);
    public static void position_yz(this Transform transform, Vector2 val) => transform.position = new Vector3(transform.position.x, val.x, val.y);
    public static void position_xz(this Transform transform, Vector2 val) => transform.position = new Vector3(val.x, transform.position.y, val.y);

    public static void position_x(this Transform transform, float val) => transform.position = new Vector3(val, transform.position.y, transform.position.z);
    public static void position_y(this Transform transform, float val) => transform.position = new Vector3(transform.position.x, val, transform.position.z);
    public static void position_z(this Transform transform, float val) => transform.position = new Vector3(transform.position.x, transform.position.y, val);


}
