using Cinemachine;
using UnityEngine;

public class CMCameraController : MonoBehaviour
{
    [SerializeField] private GameObject cmFreelookGameObject;
    [SerializeField] private GameObject tpVCNormalGameObject;
    [SerializeField] private GameObject tpVCAimGameObject;

    private CinemachineFreeLook cmFreelook;
    private CinemachineVirtualCamera cmVCNormal;
    private CinemachineVirtualCamera cmVCAim;



    void Awake()
    {
        cmFreelook = cmFreelookGameObject.GetComponent<CinemachineFreeLook>();
        cmVCNormal = tpVCNormalGameObject.gameObject.GetComponent<CinemachineVirtualCamera>();
        cmVCAim = tpVCAimGameObject.gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    public void setFreelookPriority(int setTo)
    {
        cmFreelook.Priority = setTo;
    }

    public void setCmVcNormalPriority(int setTo)
    {
        cmVCNormal.Priority = setTo;
    }

    public void setCmVcAimPriority(int setTo)
    {
        cmVCAim.Priority = setTo;
    }

    public void moveBallCamToAimCam()
    {
        cmFreelook.transform.position = cmVCNormal.transform.position;
    }
}
