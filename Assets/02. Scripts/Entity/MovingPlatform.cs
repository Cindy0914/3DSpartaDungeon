using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Direction direction;
    [SerializeField] private Transform tr;
    [SerializeField] private float maxDistance;
    [SerializeField] private float moveSpeed;

    private Vector3[] movePoint;
    private Vector3 prevPos;
    private int currentIndex;

    private void Start()
    {
        movePoint = new[]
        {
            tr.position + GetDirection() * maxDistance,
            tr.position + GetDirection() * -maxDistance,
        };
        
        StartCoroutine(moveCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    
    private Vector3 GetDirection()
    {
        return direction switch
        {
            Direction.X    => tr.right,
            Direction.Y    => tr.up,
            Direction.Z    => tr.forward,
            _                  => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private IEnumerator moveCoroutine()
    {
        while (true)
        {
            tr.position = Vector3.MoveTowards(tr.position, movePoint[currentIndex], moveSpeed * Time.deltaTime);
            if (tr.position == movePoint[currentIndex])
            {
                currentIndex = (currentIndex + 1) % movePoint.Length;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.SetParent(tr);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}

public enum Direction
{
    X,
    Y,
    Z,
}