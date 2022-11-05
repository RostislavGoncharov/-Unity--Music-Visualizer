/*
 * This class controls the post-processing effects and should be attached to the main camera.
 * It currently controls lens distortion but can be used to control any other post-processing effect as well.
 */

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    public PostProcessVolume volume;
    LensDistortion lensDistortionSettings;

    private void Update()
    {
        if (!AudioBase.isActive)
        {
            return;
        }

        if (volume.profile.TryGetSettings<LensDistortion>(out lensDistortionSettings))
        {
            if (AudioBase.normalizedAverageVolume >= 0.5f)
            {
                lensDistortionSettings.intensity.Override(lensDistortionSettings.intensity - 10 * Time.deltaTime);
                lensDistortionSettings.scale.Override(lensDistortionSettings.scale + 0.1f * Time.deltaTime);

                if (lensDistortionSettings.scale >= 1.5f)
                {
                    lensDistortionSettings.scale.Override(1.5f);
                }
            }

            else if (AudioBase.normalizedAverageVolume <= 0.3f)
            {
                lensDistortionSettings.intensity.Override(lensDistortionSettings.intensity + 5 * Time.deltaTime);

                if (lensDistortionSettings.intensity > 0)
                {
                    lensDistortionSettings.intensity.Override(0);
                }

                lensDistortionSettings.scale.Override(lensDistortionSettings.scale - 0.1f * Time.deltaTime);

                if (lensDistortionSettings.scale <= 0.5f)
                {
                    lensDistortionSettings.scale.Override(0.5f);
                }
            }
        }
    }
}
