using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Words
{
    [RequireComponent(typeof(Collider2D))]
    public class Word : MonoBehaviour
    {
        public GameObject spritePrefab;
        public Connotation connotation;
        public bool isEssential;

        
        public string LayerWhenInBubble;
        public string LayerWhenOutsideOfBubble;

        [SerializeField] private float scaleMultiplierMin = 1.5f;
        [SerializeField] private float scaleMultiplierMax = 2f;

        void Start()
        {
            SetLayer(LayerMask.NameToLayer(LayerWhenInBubble));
        }


        void Update()
        {
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Bubble>() == null)
                return;

            SetLayer(LayerMask.NameToLayer(LayerWhenOutsideOfBubble));
            transform.localScale = collision.transform.localScale * Random.Range(scaleMultiplierMin, scaleMultiplierMax);
        }

        private void SetLayer(int layer)
        {
            gameObject.layer = layer;
            foreach (Collider2D collider in gameObject.GetComponentsInChildren<Collider2D>())
            {
                collider.gameObject.layer = layer;
            }
        }
    }
}