using System.Collections;
using UnityEngine;

public class ToxicWaste : MonoBehaviour, IResettable
{
    private bool isTargeted;
    private float currentImpulseSpeed;
    private float friction;
    private float toxicityValue;

    public void Initialize(ToxicWasteData wasteData, Vector3 ejectionDirection)
    {
        float randomSpeedModifier = Random.Range(0.7f, 1.3f);
        currentImpulseSpeed = wasteData.ejectionSpeed * randomSpeedModifier;
        friction = wasteData.friction;
        isTargeted = false;
        toxicityValue = wasteData.toxicAmount;

        StartCoroutine(SlideCoroutine(ejectionDirection));
    }

    private IEnumerator SlideCoroutine(Vector3 ejectionDirection)
    {
        while (currentImpulseSpeed > 0.1f)
        {
            transform.position += ejectionDirection * currentImpulseSpeed * Time.deltaTime;
            currentImpulseSpeed -= friction * Time.deltaTime;

            yield return null;
        }
    }

    public void SetIsTargeted(bool set)
    {
        isTargeted = set;
    }

    public bool GetIsTargeted()
    {
        return isTargeted;
    }
    
    public float GetToxicityValue()
    {
        return toxicityValue;
    }

    public void Reset()
    {
        isTargeted = false;
        currentImpulseSpeed = 0f;
        toxicityValue = 0f;
    }
}
