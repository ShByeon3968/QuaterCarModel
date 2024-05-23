using System;
using System.Collections.Generic;
using UnityEngine;

public class QuarterCarModel : MonoBehaviour
{
    // ���� ���� (�ʱ� ���� �� �ý��� �Ķ����)
    public float m1 = 250.0f; // ������ ����
    public float m2 = 50.0f;  // ������ ����
    public float k1 = 15000.0f; // ������ ���
    public float k2 = 200000.0f; // Ÿ�̾� ����
    public float c1 = 1000.0f; // ������� ���� ���
    public float c2 = 2000.0f; // Ÿ�̾� ���� ���

    private float[] state = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f }; // �ʱ� ���� [x1, v1, x2, v2]

    public float totalTime = 5.0f; // ��ü �ùķ��̼� �ð�
    public float dt = 0.01f; // Ÿ�� ����

    private void Start()
    {
        
    }

    private void Simulate()
    {
        float time = 0.0f;
        while (time <= totalTime)
        {
            float[] k1 = QuarterCarModelEquations(time, state);
            float[] k2 = QuarterCarModelEquations(time + dt / 2.0f, AddVectors(state, MultiplyVector(k1, dt / 2.0f)));
            float[] k3 = QuarterCarModelEquations(time + dt / 2.0f, AddVectors(state, MultiplyVector(k2, dt / 2.0f)));
            float[] k4 = QuarterCarModelEquations(time + dt, AddVectors(state, MultiplyVector(k3, dt)));

            for (int i = 0; i < state.Length; i++)
            {
                state[i] += dt / 6.0f * (k1[i] + 2.0f * k2[i] + 2.0f * k3[i] + k4[i]);
            }

            time += dt;

            Debug.Log($"Time: {time:F2} s, x1: {state[0]:F4} m, v1: {state[1]:F4} m/s, x2: {state[2]:F4} m, v2: {state[3]:F4} m/s");
        }
    }


    private float RoadInput(float t)
    {
        return 0.01f * Mathf.Sin(2.0f * Mathf.PI * 1.0f * t); // ���� 0.01m, ���ļ� 1Hz
    }

    private float[] QuarterCarModelEquations(float t, float[] y)
    {
        float x1 = y[0];
        float v1 = y[1];
        float x2 = y[2];
        float v2 = y[3];
        float z = RoadInput(t);

        float dx1_dt = v1;
        float dv1_dt = (1 / m1) * (-k1 * (x1 - x2) - c1 * (v1 - v2));
        float dx2_dt = v2;
        float dv2_dt = (1 / m2) * (k1 * (x1 - x2) + c1 * (v1 - v2) - k2 * (x2 - z) - c2 * (v2 - 0));

        return new float[] { dx1_dt, dv1_dt, dx2_dt, dv2_dt };
    }

    private float[] AddVectors(float[] a, float[] b)
    {
        float[] result = new float[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] + b[i];
        }
        return result;
    }

    private float[] MultiplyVector(float[] a, float b)
    {
        float[] result = new float[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] * b;
        }
        return result;
    }
}
