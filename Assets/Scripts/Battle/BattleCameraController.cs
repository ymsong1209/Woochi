using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BattleCameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera focusCamera;

    [Header("Post Process")]
    [SerializeField] private PostProcessVolume postProcessVolume;

    [Header("Impulse")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeAmount = 0.1f;

    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += FocusIn;
        BattleManager.GetInstance.OnFocusEnd += FocusOut;
        BattleManager.GetInstance.OnShakeCamera += Shake;
    }

    public void FocusIn()
    {
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HUD"));
        postProcessVolume.enabled = true;

        // targetGroup.m_Targets = targets.ToArray();
    }

    public void FocusOut()
    {
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("HUD");
        postProcessVolume.enabled = false;

        // targets.Clear();
    }

    public void Shake(bool _isHit, bool _isCrit)
    {
        if(_isHit)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCamera(_isCrit));
        }
    }

    IEnumerator ShakeCamera(bool _isCrit)
    {
        float elapsed = 0f;

        if(_isCrit)
        {
            shakeAmount *= 2;
        }

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            focusCamera.transform.localPosition = new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        focusCamera.transform.localPosition = Vector3.zero;
    }
}
