using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcessCircle : MonoBehaviour
{
    float lastSliderUpdate;
    private bool finished;
    private Slider slider;
    private float coolDown = 0.1f;
    private float endCoolDown = 1.5f;
    [SerializeField] private TextMeshProUGUI textField;

    void Start()
    {
        lastSliderUpdate = Time.realtimeSinceStartup;
        slider = gameObject.GetComponent<Slider>();
    }

    void Update()
    {
        if (finished)
        {
            if (Time.realtimeSinceStartup - lastSliderUpdate >= endCoolDown)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (Time.realtimeSinceStartup - lastSliderUpdate >= coolDown)
        {
            lastSliderUpdate = Time.realtimeSinceStartup;
            slider.value += 0.01f;
            textField.text = Mathf.Round(slider.value * 100) + "%";
            if (slider.value >= 1)
            {
                finished = true;
            }
        }
    }
}
