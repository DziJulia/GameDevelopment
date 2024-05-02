using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Pica pica;
    public Image fillImage;
    public int fullLife;

    void Start()
    {
        fullLife = pica.picaLife;
    }
    
    // Update is called once per frame
    void Update()
    {
        fillImage.color = new Color(1, 0.5f, 0); 
        slider.value = slider.maxValue - pica.picaLife;
        float lifePercentage = ((slider.value - pica.picaLife) / slider.value) * 100;
        Debug.Log(" flifePercentage" +  lifePercentage);
        if (lifePercentage >= 25) 
        {
            fillImage.color = Color.red;
        }
    }
}