using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class SetRendererOnStart : MonoBehaviour
    {
        [SerializeField] private bool editorValue;
        [SerializeField] private bool playValue;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.enabled = playValue;
        }

        private void OnValidate()
        {            
            _renderer = GetComponent<Renderer>();
            _renderer.enabled = editorValue;
        }
    }
}
