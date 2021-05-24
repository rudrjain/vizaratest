using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Android;

public class RealWorldWeather : MonoBehaviour
{
	public Text lat,temp,wind,des,pres;
	public GameObject gpsPanel, introPanel,i1,i2,sidePanel;
	private void Awake()
	{
		if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
		{
			Permission.RequestUserPermission(Permission.FineLocation);
			Permission.RequestUserPermission(Permission.CoarseLocation);
		}
	}
	IEnumerator Start()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
		{
			Permission.RequestUserPermission(Permission.FineLocation);
			Permission.RequestUserPermission(Permission.CoarseLocation);
			Debug.Log("no permission");
			gpsPanel.SetActive(true);
			//txt.text = "FAIL";
			yield break;

		}


		// Start service before querying location
		Input.location.Start();
		Debug.Log("Asking Permission");
		gpsPanel.SetActive(false);
		introPanel.SetActive(true);
		i1.SetActive(true);

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
			//txt.text = "PASS";
			latitude = Input.location.lastData.latitude.ToString();
			longitude = Input.location.lastData.longitude.ToString();
			GetRealWeather();
			
		}

		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();
	}

	/*
		In order to use this API, you need to register on the website.

		Source: https://openweathermap.org

		Request by city: api.openweathermap.org/data/2.5/weather?q={city id}&appid={your api key}
		Request by lat-long: api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={your api key}

		Api response docs: https://openweathermap.org/current
	*/

	public string apiKey = "81be056bd125419a064f64caefd7efda";

	public string city;
	public bool useLatLng = false;
	public string latitude;
	public string longitude;

	public void GetRealWeather () {
		string uri = "api.openweathermap.org/data/2.5/weather?";
		if (useLatLng) {
			uri += "lat=" + latitude + "&lon=" + longitude + "&appid=" + apiKey;
		} else {
			uri += "q=" + city + "&appid=" + apiKey;
		}
		StartCoroutine (GetWeatherCoroutine (uri));
	}

	IEnumerator GetWeatherCoroutine (string uri) {
		using (UnityWebRequest webRequest = UnityWebRequest.Get (uri)) {
			yield return webRequest.SendWebRequest ();
			if (webRequest.isNetworkError) {
				Debug.Log ("Web request error: " + webRequest.error);
			} else {
				ParseJson (webRequest.downloadHandler.text);
			}
		}
	}

	WeatherStatus ParseJson (string json) {
		WeatherStatus weather = new WeatherStatus ();
		try {
			dynamic obj = JObject.Parse (json);

			weather.weatherId = obj.weather[0].id;
			weather.main = obj.weather[0].main;
			weather.description = obj.weather[0].description;
			weather.temperature = obj.main.temp;
			weather.pressure = obj.main.pressure;
			weather.windSpeed = obj.wind.speed;

		} catch (Exception e) {
			Debug.Log (e.StackTrace);
		}
		
		Debug.Log ("Temp in °C: " + weather.Celsius ());
		Debug.Log ("Wind speed: " + weather.windSpeed);

		temp.text = "Temp in °C: " + weather.Celsius().ToString();
		lat.text = "Lat : " + latitude + " Long : " + longitude;
		wind.text = " Wind Speed in m/s : " + weather.windSpeed.ToString();
		des.text = "Description : " + weather.description.ToString();
		pres.text = "Pressure in hPa : " + weather.pressure.ToString();

		return weather;
	}

	public void continuei1()
    {
		i1.SetActive(false);
		i2.SetActive(true);
		sidePanel.SetActive(true);
    }

	public void continuei2()
    {
		i2.SetActive(false);
		introPanel.SetActive(false);
		//i1.SetActive(false);

    }

}