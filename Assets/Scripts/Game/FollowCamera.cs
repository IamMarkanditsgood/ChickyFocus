using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;         // Посилання на кульку
    [SerializeField] private float yOffset = 2f;       // Відступ камери над кулькою
    [SerializeField] private float smoothSpeed = 5f;   // Швидкість згладжування

    private float highestY; // Найвище положення, яке бачила камера

    private void Start()
    {
        if (target != null)
        {
            highestY = target.position.y + yOffset;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float desiredY = target.position.y + yOffset;

        // Якщо кулька піднялася вище — оновлюємо позицію
        if (desiredY > highestY)
        {
            highestY = desiredY;
        }

        Vector3 newPos = new Vector3(transform.position.x, highestY, transform.position.z);

        // Плавне переміщення камери
        transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
    }
}
