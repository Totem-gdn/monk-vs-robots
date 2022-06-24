using Opsive.Shared.Events;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject characterObject;

    [SerializeField] private Camera mainCharacterCamera;
    [SerializeField] private Camera respawnCamera;
    [SerializeField] private Transform templeObjectTransform;
    [SerializeField] private Transform cameraMoveTransform;
    [SerializeField] private float cameraTransitionDuration;
    [SerializeField] private float respawnCameraFieldOfView;
    [SerializeField] private Ease inEaseType;
    [SerializeField] private Ease outEaseType;
    [SerializeField] private float cameraWingTime = 1.3f; //by one side
    [SerializeField] private float cameraWingSpeedMod = 10;

    private void Awake()
    {
        EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(characterObject, "OnDeath", OnPlayerDeath);
        EventHandler.RegisterEvent(characterObject, "OnRespawn", OnPlayerRespawn);
    }

    private void OnPlayerDeath(Vector3 position, Vector3 force, GameObject attacker)
    {
        print($"[ITD-RespawnManager] On player death!");
        respawnCamera.transform.position = mainCharacterCamera.transform.position;
        respawnCamera.transform.rotation = mainCharacterCamera.transform.rotation;
        StartCoroutine(DeathDelayAndMoveCamera());
    }

    private void OnPlayerRespawn()
    {
        print($"[ITD-RespawnManager] On player respawn!");
        StartCoroutine(RespawnDelayAndMoveCamera());
    }

    public void ShowRespawnCamera()
    {
        ShowHideRespawnCamera(true);
    }

    public void HideRespawnCamera()
    {
        ShowHideRespawnCamera(false);
    }

    private void ShowHideRespawnCamera(bool isShow)
    {
        mainCharacterCamera.enabled = !isShow;
        respawnCamera.enabled = isShow;
    }

    private Vector3[] GeneratePrimitiveArcPath(Vector3 start, Vector3 end)
    {
        Vector3[] path = new Vector3[3];
        path[0] = start;
        path[2] = end;

        Vector3 midPoint = (path[0] + path[2]) / 2;
        midPoint.y += 70;

        path[1] = midPoint;
        return path;
    }

    private IEnumerator DeathDelayAndMoveCamera()
    {
        yield return new WaitForSeconds(0.5f);
        ShowRespawnCamera();
        respawnCamera.DOFieldOfView(respawnCameraFieldOfView, cameraTransitionDuration).SetEase(inEaseType);
        respawnCamera.transform.DORotateQuaternion(cameraMoveTransform.rotation, cameraTransitionDuration).SetEase(inEaseType);

        respawnCamera.transform
            .DOMove(cameraMoveTransform.position, cameraTransitionDuration)
            .SetEase(inEaseType)
            .OnComplete(() => 
        {
            StartCoroutine(RespawnCameraSwingSideToSide());
        });
        //respawnCamera.transform.DOPath(path, cameraTransitionDuration).SetEase(inEaseType);
    }

    private IEnumerator RespawnDelayAndMoveCamera()
    {
        yield return null;
        respawnCamera.transform.DORotateQuaternion(mainCharacterCamera.transform.rotation, cameraTransitionDuration).SetEase(outEaseType);
        respawnCamera.DOFieldOfView(mainCharacterCamera.fieldOfView, cameraTransitionDuration).SetEase(outEaseType);
        respawnCamera.transform
            .DOMove(mainCharacterCamera.transform.position, cameraTransitionDuration)
            .SetEase(outEaseType)
            .OnComplete(() =>
        {
            HideRespawnCamera();
        });
    }

    private IEnumerator RespawnCameraSwingSideToSide()
    {
        float time = 0;
        while(time <= cameraWingTime)
        {
            respawnCamera.transform.RotateAround(templeObjectTransform.position, Vector3.up, Time.deltaTime * cameraWingSpeedMod);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(characterObject, "OnDeath", OnPlayerDeath);
        EventHandler.UnregisterEvent(characterObject, "OnRespawn", OnPlayerRespawn);
    }
}
