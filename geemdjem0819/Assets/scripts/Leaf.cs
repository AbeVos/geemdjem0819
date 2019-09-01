using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public float meanSize = 2f;
    public float stdSize = 1f;
    public float growthFalloff = 1f;

    float targetSize;
    float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        targetSize = SampleGaussian(meanSize, stdSize);
        timeAlive = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;        
    }

    public void UpdateGrowth()
    {
        transform.localScale = Vector3.one * targetSize * (1 - Mathf.Exp(-timeAlive / growthFalloff));
    }

    /// Sample a value from a Normal distribution with given mean and std.
    private float SampleGaussian(float mean, float std)
    {
        var u1 = Random.value;
        var u2 = Random.value;

        var c = Mathf.Sqrt(-2 * Mathf.Log(u1));
        var z0 = Mathf.Cos(2 * Mathf.PI * u2);

        return std * z0 + mean;
    }
}