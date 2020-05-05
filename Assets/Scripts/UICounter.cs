﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UICounter : MonoBehaviour
{
	TextMeshProUGUI text;
	string Format;
	int _value = 0;
	int _valueTarget = 0;
	int _valueOrigin = 0;

	public delegate void CallbackDelegate(int value);

	public CallbackDelegate Callback;

	/// <summary>
	/// Time it takes for the Counter to Get to the Current Value
	/// </summary>
	public float Duration = 2f;

	public int testing;

	float time = 0;

	/// <summary>
	/// Value of the Counter
	/// </summary>
	/// <value></value>
	[field: SerializeField]
	public int Value{
		get{
			return _value;
		}

		set{
			_valueOrigin = _value;
			_valueTarget = value;
			time = 0;
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
		Format = text.text;
	}

	// Update is called once per frame
	void Update()
	{
		if(testing != _valueTarget)
			Value = testing;

		//Increase the current Time until it hits one
		if(time < 1)
			time += Time.deltaTime / Duration;
		else{
			time = 1;

			//Check if this is the frame in which it hits one and will call the Callback letting the 
			if(_value != _valueTarget){
				if(Callback != null)
					Callback(_valueTarget);
			}
		}

		// Update the current Value to Interpolate between the Origin and the Target
		if(_value != _valueTarget)
			_value = Mathf.RoundToInt(Mathf.SmoothStep(_valueOrigin, _valueTarget, time));

		text.text = string.Format(Format, _value);
	}
}