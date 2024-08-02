using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Focus")]
    List<BaseCharacter> foucsCharacters = new List<BaseCharacter>();

    [Header("Object")]
    [SerializeField] private GameObject battleBg;

    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += FocusIn;
        BattleManager.GetInstance.OnFocusEnd += FocusOut;
        BattleManager.GetInstance.OnFocusEnter += FocusEnter;
        BattleManager.GetInstance.OnShakeCamera += Shake;

        battleBg.SetActive(false);
    }

    public void FocusIn()
    {
        battleBg.SetActive(true);

        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HUD"));
        postProcessVolume.enabled = true;

        Place();
    }

    public void FocusOut()
    {
        battleBg.SetActive(false);

        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("HUD");
        postProcessVolume.enabled = false;

        foucsCharacters.Clear();
    }

    public void FocusEnter(BaseCharacter character)
    {
        foucsCharacters.Add(character);
    }

    private void Place()
    {
        var sortedCharacters = foucsCharacters.
            OrderByDescending(character => character.IsAlly)
            .ThenBy(character => character.RowOrder)
            .ToList();

        float allyX = -2.5f;
        float enemyX = 2.5f;

        foreach (var character in sortedCharacters)
        {
            Vector3 battlePos = character.transform.position;

            if (character.IsAlly)
            {
                battlePos.x = allyX;
                allyX -= 2.5f;
            }
            else
            {
                battlePos.x = enemyX;
                enemyX += 2.5f;
            }
        }
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
