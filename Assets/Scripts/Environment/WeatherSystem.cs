using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    // ������Ч��
    public ParticleSystem rainParticles;
    // ѩ����Ч��
    public ParticleSystem snowParticles;

    // ö�ٶ�������״̬
    private enum WeatherState
    {
        Sunny,Rainy,Snowy,Foggy
    };
    // ��ǰ������״̬
    private WeatherState currentWeatherState;

    // �´������仯ʱ����
    private float timeUntilNextWeatherChange = 0f;
    // ��С�����仯ʱ����
    public float minWeatherChangeTime = 10f;
    // ��������仯ʱ����
    public float maxWeatherChangeTime = 30f;

    // Start is called before the first frame update
    private void Start()
    {
        // ��ʼʱĬ������
        SetWeatherState(WeatherState.Sunny);
    }

    private void Update()
    {
        timeUntilNextWeatherChange -= Time.deltaTime;
        if (timeUntilNextWeatherChange <= 0)
        {
            // ����л������ܱ���ԭ����״̬
            SetRandomWeather();
        }
    }

    private void SetWeatherState(WeatherState weatherState)
    {
        // ��Ӧʵ��Ч����***�����***��
        switch (weatherState)
        {
            case WeatherState.Sunny:
                rainParticles.Stop();
                snowParticles.Stop();
                break;
            case WeatherState.Rainy:
                rainParticles.Play();
                snowParticles.Stop();
                break;
            case WeatherState.Snowy:
                snowParticles.Play();
                rainParticles.Stop();
                break;
            case WeatherState.Foggy: 
                rainParticles.Stop();
                snowParticles.Stop();
                break;
            default: break;
        }
        currentWeatherState = weatherState;
    }

    private void SetRandomWeather()
    {
        int randomValue = Random.Range(0, 3);
        float randomTime = Random.Range(minWeatherChangeTime, maxWeatherChangeTime);
        if (randomValue == 0)
        {
            SetWeatherState(WeatherState.Sunny);
            timeUntilNextWeatherChange = randomTime;
        }
        else if (randomValue == 1)
        {
            SetWeatherState(WeatherState.Rainy);
            // ����ʱ��ϳ�
            timeUntilNextWeatherChange = randomTime * 2f;
        }
        else if (randomValue == 2)
        {
            SetWeatherState(WeatherState.Snowy);
            // ��ѩʱ��϶�
            timeUntilNextWeatherChange = randomTime * 1.5f;
        }
    }
}
