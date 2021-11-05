using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLevelManager : Singleton<WorldLevelManager>
{
    public List<Transform> spawnPoints;
    int currentIndex = 0;
    public List<Transform> destinations;
    public Transform currentSpawnpoint;
    public Transform currentDestination;

    [Range(0,100)]
    [SerializeField] float currentProgress;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        spawnPoints = ValidateWaypoints(spawnPoints);
        destinations = ValidateWaypoints(destinations);
        currentSpawnpoint = spawnPoints[currentIndex];
        currentDestination = destinations[currentIndex];
    }
    List<Transform> ValidateWaypoints(List<Transform> transforms) {
        List<Transform> valids = new List<Transform>();
        foreach(var point in transforms) {
            if (point != null) {
                valids.Add(point);
            }
        }

        foreach(var position in valids) {
            position.position = new Vector3(position.position.x,position.position.y,0);
        }

        transforms.Clear();
        return valids;
    }
    public float PlayerCurrentProgress(Vector3 _currentPosition) {
        Debug.DrawLine(currentDestination.position,currentSpawnpoint.position);
        Vector3 current = new Vector3(_currentPosition.x,0,0);
        Vector3 spawn = new Vector3(currentSpawnpoint.position.x,0,0);
        Vector3 dest = new Vector3(currentDestination.position.x,0,0);
        float std = Vector3.Distance(spawn,dest);
        if ((_currentPosition.x >= spawn.x
            && _currentPosition.x <= dest.x)
            || (_currentPosition.x <= spawn.x
                && _currentPosition.x >= dest.x)) {
            currentProgress = Mathf.RoundToInt((Vector3.Distance(current,spawn) * (1 / std)) * 100);
            return currentProgress;
        } else {
            if (current.x < dest.x) {
                if(current.x < spawn.x) {
                    return currentProgress = 0;
                } else {
                    return currentProgress = 100;
                }
            } else {
                if (current.x > spawn.x) {
                    return currentProgress = 100;
                } else {
                    return currentProgress = 0;
                }
            }
        }
    }
    public Vector3 GetCurrentSpawnPoint() {
        return spawnPoints[currentIndex].position;
    }
}
