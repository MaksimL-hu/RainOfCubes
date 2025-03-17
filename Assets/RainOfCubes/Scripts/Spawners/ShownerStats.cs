using TMPro;
using UnityEngine;

public class ShownerStats<Spawner> : MonoBehaviour where Spawner : SpawnObject
{
    [SerializeField] private TextMeshProUGUI _objectGottenText;
    [SerializeField] private TextMeshProUGUI _objectSpawnedText;
    [SerializeField] private TextMeshProUGUI _objectActiveText;
    [SerializeField] private GenericSpawner<Spawner> _spawner;

    private int _objectGottenValue;
    private int _objectSpawnedValue;

    private void OnEnable()
    {
        _spawner.ObjectGotten += ChangeObjectGottenValue;
        _spawner.ObjectSpawned += ChangeObjectSpawnedValue;
        _spawner.CountActiveObjectChanched += ChangeObjectActiveValue;
    }

    private void OnDisable()
    {
        _spawner.ObjectGotten -= ChangeObjectGottenValue;
        _spawner.ObjectSpawned -= ChangeObjectSpawnedValue;
        _spawner.CountActiveObjectChanched -= ChangeObjectActiveValue;
    }

    private void ChangeObjectGottenValue()
    {
        _objectGottenValue++;
        _objectGottenText.text = "Количество заспавненых объектов за всё время: " + _objectGottenValue;
    }

    private void ChangeObjectSpawnedValue()
    {
        _objectSpawnedValue++;
        _objectSpawnedText.text = "Количество созданных объектов : " + _objectSpawnedValue;
    }

    private void ChangeObjectActiveValue(int value)
    {
        _objectActiveText.text = "Количество активных объектов : " + value;
    }
}