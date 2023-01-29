using UnityEngine;

namespace StrikeOnline.UpdatedPlayer
{
    public class PlayerGroundCheck : MonoBehaviour
    {
        #region Private Fields

        private PlayerController _playerController;
        [SerializeField]private LayerMask canJump;

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {

            _playerController.SetGroundedState(Physics.CheckBox(transform.position,new Vector3(0.6f,0.1f,0.6f),Quaternion.identity,canJump));
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color=Color.red;
            Gizmos.DrawCube(transform.position,new Vector3(0.6f,0.1f,0.6f));
        }

        #endregion
    }
}