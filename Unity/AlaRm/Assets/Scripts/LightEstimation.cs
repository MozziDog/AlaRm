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
        // frameReceived�� AR ī�޶�κ��� �������� ���ŵ� ������ ����Ǵ� �̺�Ʈ
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameChanged;
    }

    private void FrameChanged(ARCameraFrameEventArgs args)
    {
        // ����� ��¿�
        float lightIntensity = 0f;
        float colorTemp = 0f;
        Color colorCorrection = new Color();
        Vector3 mainLightDirection = Vector3.zero;
        float averageMainLightBrightness = 0f;

        // lightEstimation�� nullable�̱� ������ HasValue�� üũ �ʿ�
        // ��� ���
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            light.intensity = args.lightEstimation.averageBrightness.Value;
            lightIntensity = args.lightEstimation.averageBrightness.Value;
        }

        // ��� ���µ�
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            light.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }

        // �������� (RGBA)
        if (args.lightEstimation.colorCorrection.HasValue)
        {
            light.color = args.lightEstimation.colorCorrection.Value;
            colorCorrection = args.lightEstimation.colorCorrection.Value;
        }

        // ����� �ֿ� ���� ����
        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            light.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
        }

        // �� ���� ����ġ (���� : ���)
        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            light.intensity = args.lightEstimation.averageMainLightBrightness.Value;
        }

        // ���� 2���� ���� �����ĸ� ����� �ֺ� ��� ���� ����
        if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = args.lightEstimation.ambientSphericalHarmonics.Value;
        }

        // ����� ���
        debugOut.text = $"lightIntensity: {lightIntensity}\nTemp:{colorTemp}\ncolor:{colorCorrection}\nmainLightDir:{mainLightDirection}\nmainLightBri:{averageMainLightBrightness}";
        
    }
}

