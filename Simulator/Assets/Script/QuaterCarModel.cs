using System.Collections.Generic;
using UnityEngine;

public class QuarterCarModel : MonoBehaviour
{
    public float m1 = 250f;  // 차체 질량 (kg)
    public float m2 = 50f;   // 휠 질량 (kg)
    public float k1 = 80000f;  // 서스펜션 스프링 상수 (N/m)
    public float k2 = 500000f;  // 타이어 스프링 상수 (N/m)
    public float c1 = 1000f;   // 서스펜션 댐퍼 계수 (Ns/m)
    public float c2 = 2000f;   // 타이어 댐퍼 계수 (Ns/m)

    public float bumpstart = 2.0f;  // 요철 시작 시간 (s)
    public float bumpend = 2.5f;    // 요철 종료 시간 (s)
    public float bumpamp = 0.001f;  // 요철 진폭 (m)
    public float simulationTime = 5.0f;
    public int steps = 1000;

    private List<float> times;
    private List<Vector4> states;  // [x1, v1, x2, v2]

    public void StartSimulation()
    {
        times = new List<float>();
        states = new List<Vector4>();

        float dt = simulationTime / steps;
        Vector4 state = new Vector4(0, 0, 0, 0);  // 초기 조건 [x1, v1, x2, v2]

        for (float t = 0; t <= simulationTime; t += dt)
        {
            times.Add(t);
            states.Add(state);

            Vector4 k1 = dt * QuarterCarDynamics(t, state);
            Vector4 k2 = dt * QuarterCarDynamics(t + 0.5f * dt, state + 0.5f * k1);
            Vector4 k3 = dt * QuarterCarDynamics(t + 0.5f * dt, state + 0.5f * k2);
            Vector4 k4 = dt * QuarterCarDynamics(t + dt, state + k3);

            state = state + (1.0f / 6.0f) * (k1 + 2 * k2 + 2 * k3 + k4);
        }
    }

    Vector4 QuarterCarDynamics(float t, Vector4 y)
    {
        float x1 = y.x;
        float v1 = y.y;
        float x2 = y.z;
        float v2 = y.w;
        float z = RoadInput(t);

        float dx1_dt = v1;
        float dv1_dt = (1 / m1) * (-k1 * (x1 - x2) - c1 * (v1 - v2));
        float dx2_dt = v2;
        float dv2_dt = (1 / m2) * (k1 * (x1 - x2) + c1 * (v1 - v2) - k2 * (x2 - z) - c2 * v2);

        return new Vector4(dx1_dt, dv1_dt, dx2_dt, dv2_dt);
    }

    float RoadInput(float t)
    {
        if (bumpstart <= t && t <= bumpend)
        {
            return bumpamp * Mathf.Sin(Mathf.PI * (t - bumpstart) / (bumpend - bumpstart));
        }
        else
        {
            return 0;
        }
    }

    void SetParameters()
    {

    }

    public List<float> GetTimes() => times;
    public List<Vector4> GetStates() => states;
}
