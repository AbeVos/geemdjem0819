using UnityEngine;

public class DayNightLookat : MonoBehaviour
{
    private Vector3 blerp;
    private void Start()
    {
        var position = gameObject.transform.position;
        blerp = new Vector3 { x = position.x,
                              y = position.y,
                              z = position.z -3};
    }
    private void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,blerp,0.1f);
    }
}
