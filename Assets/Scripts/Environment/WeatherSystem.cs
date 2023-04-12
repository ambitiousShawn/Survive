using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    // 雨粒子效果
    public ParticleSystem rainParticles;
    // 雪粒子效果
    public ParticleSystem snowParticles;

    // 枚举定义天气状态
    private enum WeatherState
    {
        Sunny,Rainy,Snowy,Foggy
    };
    // 当前的天气状态
    private WeatherState currentWeatherState;

    // 下次天气变化时间间隔
    private float timeUntilNextWeatherChange = 0f;
    // 最小天气变化时间间隔
    public float minWeatherChangeTime = 10f;
    // 最大天气变化时间间隔
    public float maxWeatherChangeTime = 30f;

    // Start is called before the first frame update
    private void Start()
    {
        // 开始时默认晴天
        SetWeatherState(WeatherState.Sunny);
    }

    private void Update()
    {
        timeUntilNextWeatherChange -= Time.deltaTime;
        if (timeUntilNextWeatherChange <= 0)
        {
            // 随机切换，可能保持原天气状态
            SetRandomWeather();
        }
    }

    private void SetWeatherState(WeatherState weatherState)
    {
        // 对应实际效果（***待添加***）
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
            // 下雨时间较长
            timeUntilNextWeatherChange = randomTime * 2f;
        }
        else if (randomValue == 2)
        {
            SetWeatherState(WeatherState.Snowy);
            // 下雪时间较短
            timeUntilNextWeatherChange = randomTime * 1.5f;
        }
    }
}
