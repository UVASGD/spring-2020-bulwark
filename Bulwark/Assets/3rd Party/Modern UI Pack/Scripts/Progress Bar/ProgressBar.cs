using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("OBJECTS")]
        public Transform loadingBar;
        public Transform textPercent;

        [Header("SETTINGS")]
        public bool isOn;
        public bool restart;
        public bool invert;
        public ProgressType progressType;
        public bool displayMax = true;

        public enum ProgressType { percent, count }

        [Header("Percent-based")]
        [Range(0, 100)] public float currentPercent;
        [Range(0, 100)] public int speed;
        [Header ("Count-based")]
        [Range(0, 100)] public int currentCount;
        [Range(0, 100)] public int maxCount;

        void Start () {
            if(isOn == false) {
                UpdateProgress();
            }
        }

        void Update () {
            if (isOn == true) {
                if (currentPercent <= 100 && invert == false)
                    currentPercent += speed * Time.deltaTime;

                else if (currentPercent >= 0 && invert == true)
                    currentPercent -= speed * Time.deltaTime;

                if (currentPercent >= 100 || currentPercent >= 100 && restart == true && invert == false)
                    currentPercent = 0;

                else if (currentPercent <= 0 || currentPercent <= 0 && restart == true && invert == true)
                    currentPercent = 100;

                UpdateProgress();
            }
        }

        public void UpdateProgress () {
            switch (progressType) {
            case ProgressType.percent:
                UpdateProgress (currentPercent);
                break;
            case ProgressType.count:
                UpdateProgress (currentCount, maxCount);
                break;
            default:
                Debug.LogError ("Unknown Progress Type in ProgressBar.cs");
                break;
            }
        }

        public void UpdateProgress(float percent) {
            loadingBar.GetComponent<Image> ().fillAmount = percent / 100;
            textPercent.GetComponent<TextMeshProUGUI> ().text = ((int)percent).ToString ("F0") + "%";
        }

        public void UpdateProgress (int currentCount, int maxCount) {
            this.currentCount = currentCount;
            this.maxCount = maxCount;

            // Max Level override
            if (maxCount < 0) {
                loadingBar.GetComponent<Image> ().fillAmount = 1f;
                textPercent.GetComponent<TextMeshProUGUI> ().text = "MAX";
                return;
            }

            if (maxCount == 0) {
                loadingBar.GetComponent<Image> ().fillAmount = 0;
            } else {
                loadingBar.GetComponent<Image> ().fillAmount = (float)currentCount / maxCount;
            }

            if (displayMax) {
                textPercent.GetComponent<TextMeshProUGUI>().text = (currentCount).ToString("F0") + "/" + maxCount.ToString();
            } else {
                textPercent.GetComponent<TextMeshProUGUI>().text = (currentCount).ToString("F0");
            }
        }
    }
}