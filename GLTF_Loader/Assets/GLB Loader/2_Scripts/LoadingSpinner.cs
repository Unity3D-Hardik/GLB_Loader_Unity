using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace KetosGames.SceneTransition
{
    public class LoadingSpinner : MonoBehaviour
    {
        public float speed = 0.05f;

        void Update()
        {
            Quaternion newRotation = Quaternion.AngleAxis(180, Vector3.forward);
            transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, speed);
            if (Quaternion.Angle(transform.rotation, newRotation) < 1)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
        }
    }
}
