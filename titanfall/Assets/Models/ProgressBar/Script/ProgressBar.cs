using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]

public class ProgressBar : MonoBehaviour
{

    [Header("Title Setting")]
    public string Title;
    public Color TitleColor;
    public Font TitleFont;
    public int TitleFontSize = 10;

    [Header("Bar Setting")]
    public Color BarColor;   
    public Color BarBackGroundColor;
    public Sprite BarBackGroundSprite;
    [Range(1f, 100f)]
    public int Alert = 20;
    public int dashAlert = 2;
    public Color BarAlertColor;

    [Header("Sound Alert")]
    public AudioClip sound;
    public bool repeat = false;
    public float RepeatRate = 1f;

    private Image bar, barBackground;
    private float nextPlay;
    private AudioSource audiosource;
    private Text txtTitle;
    private float barValue;
    public bool dash = false;
    public float BarValue;
    // {
    //     get { return barValue; }

    //     set
    //     {
    //         int maxBarValue = 100;

    //         if (dash)
    //         {
    //             barValue = (float)value / 3 * 100;
    //             //barValue = Mathf.Clamp(value, 0, maxBarValue);
    //             UpdateValue(barValue); 
    //         }
    //         else 
    //         {
    //             value = Mathf.Clamp(value, 0, maxBarValue);
    //             barValue = value;
    //             UpdateValue(barValue);
    //         }

    //     }
    // }

        

    private void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
        txtTitle = transform.Find("Text").GetComponent<Text>();
        //audiosource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        txtTitle.text = Title;
        txtTitle.color = TitleColor;
        txtTitle.font = TitleFont;
        txtTitle.fontSize = TitleFontSize;

        bar.color = BarColor;
        //UpdateValue(barValue);


    }

    public void UpdateValue(float val, float maxBarValue)
    {
        bar.fillAmount = val / maxBarValue;
        txtTitle.text = Title + " " + val;

        if(dash)
        {
            //bar.fillAmount = val / 3;
            //txtTitle.text = Title + " " + val;

            if (dashAlert > val)
            {
                bar.color = BarAlertColor;
            }
            else
            {
                bar.color = BarColor;
            }

        }
        else
        {
            //bar.fillAmount = val / 100;
            //txtTitle.text = Title + " " + val + "%";

            if (Alert >= val)
            {
                bar.color = BarAlertColor;
            }
            else
            {
                bar.color = BarColor;
            }

        }

    }


    private void Update()
    {
        if (!Application.isPlaying)
        {           
            UpdateValue(100,100);
            txtTitle.color = TitleColor;
            txtTitle.font = TitleFont;
            txtTitle.fontSize = TitleFontSize;

            bar.color = BarColor;         
        }
        else
        {
            if (Alert >= barValue && Time.time > nextPlay)
            {
                nextPlay = Time.time + RepeatRate;
                //audiosource.PlayOneShot(sound);
            }
        }
    }

}
