using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloack : MonoBehaviour
{
	public float followSpeed = 2f;
	public Transform objetivo;
    private Transform cloakTransform;

	Vector3 originalPos;

	void Awake()
	{

		cloakTransform = GetComponent(typeof(Transform)) as Transform;
		
	}

	void OnEnable()
	{
		originalPos = cloakTransform.localPosition;
	}

	private void Update()
	{
		Vector3 newPosition = objetivo.position;
		newPosition.z = -5;
		transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);

	}

}
