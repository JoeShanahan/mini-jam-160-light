using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Porthole : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _mesh;

    private PlayerController _player;

    private float _portOpenValue = 100;
    private float _transitionSpeed = 100f;

    public void SetPortholeValue(float value)
    {
        _portOpenValue = Mathf.Clamp(value, 0, 100);
        _mesh.SetBlendShapeWeight(0, _portOpenValue);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindFirstObjectByType<PlayerController>();
        SetPortholeValue(0);
        OpenPorthole();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _player.transform.position;
        pos.z = -2;
        transform.position = pos;

        if (_shouldBeOpen && _portOpenValue < 100)
        {
            SetPortholeValue(_portOpenValue + (Time.deltaTime * _transitionSpeed));
        }

        if (!_shouldBeOpen && _portOpenValue > 0)
        {
            SetPortholeValue(_portOpenValue - (Time.deltaTime * _transitionSpeed));
        }
    }

    private bool _shouldBeOpen = true;

    public void ClosePorthole()
    {
        _shouldBeOpen = false;
    }
    
    public void OpenPorthole()
    {
        _shouldBeOpen = true;

    }
}
