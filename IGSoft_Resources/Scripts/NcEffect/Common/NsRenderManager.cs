// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class NsRenderManager : MonoBehaviour
{
	// Attribute ------------------------------------------------------------------------
	public		List<Component>		m_RenderEventCalls;

    public Camera cam;
	// Property -------------------------------------------------------------------------
	// Loop Function --------------------------------------------------------------------
	void Awake()
	{
	}

	void OnEnable()
	{
	}

	void OnDisable()
	{
	}

	void Start()
	{
	}

	void OnPreRender()
	{
		if (m_RenderEventCalls != null)
		{
            int cullingMask = cam != null ? cam.cullingMask : -1;
			for (int n = m_RenderEventCalls.Count-1; 0 <= n; n--)
			{
				if (m_RenderEventCalls[n] == null || ((1 << m_RenderEventCalls[n].gameObject.layer) & cullingMask) == 0)
				{
					m_RenderEventCalls.RemoveAt(n);
					continue;
				}
				m_RenderEventCalls[n].SendMessage("OnPreRender", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void OnRenderObject()
	{
	}

	void OnPostRender()
	{
		if (m_RenderEventCalls != null)
		//foreach (Component com in m_RenderEventCalls)
		for(int i = 0; i < m_RenderEventCalls.Count; ++i)
		{
			Component com = m_RenderEventCalls[i];
			if (com != null)
				com.SendMessage("OnPostRender", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Control Function -----------------------------------------------------------------
	public void AddRenderEventCall(Component tarCom)
	{
		if (m_RenderEventCalls == null)
			m_RenderEventCalls = new List<Component>();
		if (m_RenderEventCalls.Contains(tarCom) == false)
			m_RenderEventCalls.Add(tarCom);
	}

	public void RemoveRenderEventCall(Component tarCom)
	{
		if (m_RenderEventCalls == null)
			m_RenderEventCalls = new List<Component>();
		else if (m_RenderEventCalls.Contains(tarCom) == true)
			m_RenderEventCalls.Remove(tarCom);
	}
	// Event Function -------------------------------------------------------------------
}

