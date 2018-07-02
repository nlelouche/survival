using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int enemiesAmount;
    public float spawnDelay;
    public float waveDelay;
}

public class Spawner : MonoBehaviour {

    public List<Wave> waves = new List<Wave>();
    public BoxCollider2D spawnArea;


    private Wave _currentWave;
    private float _elapsedGameTime;
    private float _elapsedWaveTime;
    private int _spawnedEnemiesInWave = 0;
    private int _currentWaveIndex = 0;
	// Use this for initialization
	void Start () {
        
        _currentWave = waves[_currentWaveIndex];
        _elapsedGameTime = _elapsedWaveTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        _elapsedGameTime += Time.deltaTime;

        if (_elapsedGameTime >= _currentWave.waveDelay)
        {
            _elapsedWaveTime += Time.deltaTime;

            if ((_elapsedWaveTime >= _currentWave.spawnDelay) && _spawnedEnemiesInWave < _currentWave.enemiesAmount)
            {
                _elapsedWaveTime = 0;
                _spawnedEnemiesInWave++;

                Vector3 spawnpoint = new Vector3(spawnArea.bounds.min.x, Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0);

                GameObject go = Instantiate(_currentWave.enemyPrefab, spawnpoint, Quaternion.identity);
                go.transform.SetParent(this.transform);

                if (_spawnedEnemiesInWave >= _currentWave.enemiesAmount)
                {
                    if (_currentWaveIndex < waves.Count -1)
                    {
                        _currentWaveIndex++;
                        _currentWave = waves[_currentWaveIndex];
                        _spawnedEnemiesInWave = 0;
                        _elapsedGameTime = 0;
                        _elapsedWaveTime = 0;
                    }
                }
            }
        }

	}
}
