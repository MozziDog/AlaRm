using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;
    private new Light light;
    [SerializeField]
    private TextMeshProUGUI debugOut;

    private void Awake()
    {
        if (arCameraManager == null)
        {
            Debug.LogError("There is NO AR Camera Manager!!");
            Destroy(this);
        }

        light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        arCameraManager.frameReceived += FrameChanged;
        // frameReceived는 AR 카메라로부터 프레임이 수신될 때마다 실행되는 이벤트
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameChanged;
    }

    private void FrameChanged(ARCameraFrameEventArgs args)
    {
        // 디버그 출력용
        float lightIntensity = 0f;
        float colorTemp = 0f;
        Color colorCorrection = new Color();
        Vector3 mainLightDirection = Vector3.zero;
        float averageMainLightBrightness = 0f;

        // lightEstimation은 nullable이기 때문에 HasValue로 체크 필요
        // 평균 밝기
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            light.intensity = args.lightEstimation.averageBrightness.Value;
            lightIntensity = args.lightEstimation.averageBrightness.Value;
        }

        // 평균 색온도
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            light.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }

        // 색상정보 (RGBA)
        if (args.lightEstimation.colorCorrection.HasValue)
        {
            light.color = args.lightEstimation.colorCorrection.Value;
            colorCorrection = args.lightEstimation.colorCorrection.Value;
        }

        // 장면의 주요 광원 방향
        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            light.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
        }

        // 주 광원 추정치 (단위 : 루멘)
        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            light.intensity = args.lightEstimation.averageMainLightBrightness.Value;
        }

        // 레벨 2에서 구형 고조파를 사용해 주변 장면 조명 추정
        if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = args.lightEstimation.ambientSphericalHarmonics.Value;
        }

        // 디버그 출력
        debugOut.text = $"lightIntensity: {lightIntensity}\nTemp:{colorTemp}\ncolor:{colorCorrection}\nmainLightDir:{mainLightDirection}\nmainLightBri:{averageMainLightBrightness}";
        
    }
}

