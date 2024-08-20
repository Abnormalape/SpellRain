using System.Collections;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class AcidWordMovement : MonoBehaviour
    {

        private void Start()
        {
            StartCoroutine(AcidWordFallDown());
        }

        public IEnumerator AcidWordFallDown()
        {
            float _passedTime = 0;

            while (true)
            {
                _passedTime += Time.deltaTime;
                if (_passedTime >= 1f)
                {
                    _passedTime = 0;
                    this.transform.position += Vector3.down * 1f;
                }
                yield return null;
            }
        }
    }
}
