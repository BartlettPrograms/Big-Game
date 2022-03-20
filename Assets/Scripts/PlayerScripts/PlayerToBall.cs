using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToBall : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private GameObject characterContainerStanding;
    [SerializeField] private GameObject characterContainerBall;
    [SerializeField] private GameObject characterBall;
    [SerializeField] private GameObject characterStanding;
    [SerializeField] private GameObject vCamContainer;

    private Rigidbody rbBall;
    private Rigidbody rbStandingPlayer;
    private CMCameraController vCamController;

    private float characterHeight = 1f;


    private void Awake()
    {
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Enable();
        playerInputActions.Player.Transforming.performed += DoTransform;
        rbBall = characterBall.GetComponent<Rigidbody>();
        rbStandingPlayer = characterStanding.GetComponent<Rigidbody>();
        vCamController = vCamContainer.GetComponent<CMCameraController>();
    }

    private void DoTransform(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            if (characterContainerStanding.activeInHierarchy)
            {
                // Transform into ball code here
                // align both characters with current position
                characterBall.transform.position = characterStanding.transform.position;
                // Sets balls velocity & anglr velocty
                rbBall.angularVelocity = Vector3.zero;
                standingVelocityToBall();
                // Unfuck the camera transition
                vCamController.moveBallCamToAimCam();
                // Set Camera Transition speed via MainCam CMBrain
                //mainCamBrain.m_DefaultBlend;
                // Swap active character
                characterContainerStanding.SetActive(false);
                characterContainerBall.SetActive(true);
                // Swap active cameras
                vCamController.setFreelookPriority(30);
            }
            else if (characterContainerBall.activeInHierarchy)
            {
                // Transform into standing
                characterStanding.transform.position = calcStandingPos();
                ballVelocityToStanding();
                characterContainerBall.SetActive(false);
                characterContainerStanding.SetActive(true);
                vCamController.setFreelookPriority(5);
            }
        }
    }

    private Vector3 calcStandingPos()
    {
        Vector3 startPos = characterBall.transform.position;
        startPos.y += 0.5f * characterHeight;
        return startPos;
    }

    public void killVelocityBall()
    {
        rbBall.angularVelocity = Vector3.zero;
        rbBall.velocity = Vector3.zero;
    }

    public void ballVelocityToStanding()
    {
        rbStandingPlayer.velocity = rbBall.velocity;
    }

    public void standingVelocityToBall()
    {
        rbBall.velocity = rbStandingPlayer.velocity;
    }
}