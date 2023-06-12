using System;
using System.Collections;
using UnityEngine;

public static class Extensions {
	public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time) {
		return monoBehaviour.StartCoroutine(InvokeImpl(action, time));
	}

	private static IEnumerator InvokeImpl(Action action, float time) {
		yield return new WaitForSeconds(time);
		action();
	}

	public static void SetX(this Transform transform, float x) {
		Vector3 newPosition = new Vector3(x, transform.position.y, transform.position.z);
		transform.position = newPosition;
	}

	public static void SetY(this Transform transform, float y) {
		Vector3 newPosition = new Vector3(transform.position.x, y, transform.position.z);
		transform.position = newPosition;
	}

	public static void SetZ(this Transform transform, float z) {
		Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, z);
		transform.position = newPosition;
	}
	
	public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
	{
		return new DateTime(
			dateTime.Year,
			dateTime.Month,
			dateTime.Day,
			hours,
			minutes,
			seconds,
			milliseconds,
			dateTime.Kind);
	}
	
	public static DateTime ChangeHours(this DateTime dateTime, int hours)
	{
		return new DateTime(
			dateTime.Year,
			dateTime.Month,
			dateTime.Day,
			hours,
			dateTime.Minute,
			dateTime.Second,
			dateTime.Millisecond,
			dateTime.Kind);
	}
	
	public static DateTime ChangeMinutes(this DateTime dateTime, int minutes)
	{
		return new DateTime(
			dateTime.Year,
			dateTime.Month,
			dateTime.Day,
			dateTime.Hour,
			minutes,
			dateTime.Second,
			dateTime.Millisecond,
			dateTime.Kind);
	}
	
	public static DateTime ChangeSeconds(this DateTime dateTime, int seconds)
	{
		return new DateTime(
			dateTime.Year,
			dateTime.Month,
			dateTime.Day,
			dateTime.Hour,
			dateTime.Minute,
			seconds,
			dateTime.Millisecond,
			dateTime.Kind);
	}
	
	public static DateTime ChangeDay(this DateTime dateTime, int day)
	{
		return new DateTime(
			dateTime.Year,
			dateTime.Month,
			day,
			dateTime.Hour,
			dateTime.Minute,
			dateTime.Second,
			dateTime.Millisecond,
			dateTime.Kind);
	}
}