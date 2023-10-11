using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float predkoscRuchu = 5.0f;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        // Pobierz komponent Cinemachine Virtual Camera
        virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    void Update()
    {
        // Pobierz wejœcie gracza
        float ruchPionowy = Input.GetAxis("Vertical");

        // Zaktualizuj ustawienia Virtual Camera Extension
        virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.y += ruchPionowy * predkoscRuchu * Time.deltaTime;
    }
}