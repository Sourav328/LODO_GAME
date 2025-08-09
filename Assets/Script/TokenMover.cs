using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TokenMover : MonoBehaviour
{
    public MainPath mainPath;
    public Transform homePosition;

    public int startIndex = 0;
    public float moveSpeed = 3f;
    public float stepDelay = 0.08f;

    public bool IsHome = true;
    public int CurrentIndex { get; private set; } = -1;
    public bool IsMoving { get; private set; } = false;
    public bool IsSelectable { get; private set; } = false;

    [HideInInspector] public PlayerController owner;

    private List<Transform> tiles = new List<Transform>();

    private void Start()
    {
        if (mainPath == null)
            mainPath = FindFirstObjectByType<MainPath>();

        if (mainPath != null && mainPath.tiles.Count > 0)
            tiles = mainPath.tiles;

        owner = GetComponentInParent<PlayerController>();

        if (IsHome)
        {
            CurrentIndex = -1;
            if (homePosition != null)
                transform.position = homePosition.position;
        }
        else if (CurrentIndex >= 0 && tiles.Count > CurrentIndex)
        {
            transform.position = tiles[CurrentIndex].position;
        }
    }

    public bool CanMove(int steps)
    {
        if (tiles.Count == 0) return false;
        if (IsHome) return steps == 6;
        return (CurrentIndex + steps) < tiles.Count;
    }

    public void LeaveHome(Action onComplete)
    {
        if (tiles.Count == 0) { onComplete?.Invoke(); return; }
        IsHome = false;
        CurrentIndex = startIndex;
        StartCoroutine(MoveToTileRoutine(tiles[CurrentIndex], onComplete));
    }

    public void MoveSteps(int steps, Action onComplete)
    {
        if (IsMoving || !CanMove(steps)) { onComplete?.Invoke(); return; }
        StartCoroutine(MoveStepsRoutine(steps, onComplete));
    }

    private IEnumerator MoveStepsRoutine(int steps, Action onComplete)
    {
        IsMoving = true;

        for (int i = 0; i < steps; i++)
        {
            CurrentIndex++;
            if (CurrentIndex >= tiles.Count)
            {
                CurrentIndex = tiles.Count - 1; // Clamp to end
                break;
            }

            Transform target = tiles[CurrentIndex];
            while (Vector3.Distance(transform.position, target.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = target.position;

            yield return new WaitForSeconds(stepDelay);
        }

        CaptureOpponentsAtCurrentTile();

        IsMoving = false;
        onComplete?.Invoke();
    }

    private IEnumerator MoveToTileRoutine(Transform target, Action onComplete)
    {
        IsMoving = true;
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target.position;
        IsMoving = false;
        onComplete?.Invoke();
    }

    public void ReturnHome()
    {
        StopAllCoroutines();
        IsHome = true;
        CurrentIndex = -1;
        IsMoving = false;

        if (homePosition != null)
            transform.position = homePosition.position;
    }

    private void CaptureOpponentsAtCurrentTile()
    {
        var allTokens = FindObjectsByType<TokenMover>(FindObjectsSortMode.None);
        foreach (var other in allTokens)
        {
            if (other == this) continue;
            if (other.IsHome) continue;
            if (other.CurrentIndex == this.CurrentIndex && other.owner != this.owner)
                other.ReturnHome();
        }
    }



    public void SetSelectable(bool selectable)
    {
        IsSelectable = selectable;
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = selectable ? Color.green : Color.white;
    }

    private void OnMouseDown()
    {
        if (IsSelectable && !IsMoving)
            owner.OnTokenSelected(this);
    }
}
