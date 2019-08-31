using UnityEngine;

public class DayNightLookat : MonoBehaviour
{
    private Vector3 blerp;
    private void Start()
    {
        blerp = new Vector3 { x = gameObject.transform.position.x,
                              y = gameObject.transform.position.y,
                              z = gameObject.transform.position.z -3};
    }
    private void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,blerp,0.1f);
    }
}
