using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyParticleAfterFinish());
    }

    private IEnumerator DestroyParticleAfterFinish()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        if (ps != null)
        {
            yield return new WaitForSeconds(ps.main.duration);
        }

        Destroy(gameObject);
    }
}
