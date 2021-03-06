﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{

    public GameObject m_fogPlane;
    public Transform m_player;
    public LayerMask m_fogLayer;
    public float m_radius = 5f;
    private float m_radiusSquared { get { return m_radius * m_radius; } }

    private Mesh m_mesh;
    private Vector3[] m_vertices;
    private Color[] m_colours;
    // Use this for initialization
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, m_player.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
        {
            for (int i = 0; i < m_vertices.Length; i++)
            {
                Vector3 v = m_fogPlane.transform.TransformPoint(m_vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < m_radiusSquared)
                {
                    float alpha = Mathf.Min(m_colours[i].a, dist / m_radiusSquared);
                    m_colours[i].a = alpha;
                }
            }
            UpdateColour();
        }
    }

    void Initialise()
    {
        m_mesh = m_fogPlane.GetComponent<MeshFilter>().mesh;
        m_vertices = m_mesh.vertices;
        m_colours = new Color[m_vertices.Length];

        for (int i = 0; i < m_colours.Length; i++)
        {
            m_colours[i] = Color.white;
        }
        UpdateColour();
    }

    void UpdateColour()
    {
        m_mesh.colors = m_colours;
    }
}
