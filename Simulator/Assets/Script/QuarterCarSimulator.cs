using System.Collections.Generic;
using UnityEngine;

public class QuarterCarSimulator : MonoBehaviour
{
    public GameObject body;
    public GameObject wheel;
    public float scale_x = 160f;
    public float scale_y = 4000f;

    private List<float> times;
    private List<Vector4> states;
    private int currentIndex;

    void Start()
    {
        QuarterCarModel model = GetComponent<QuarterCarModel>();
        times = model.GetTimes();
        states = model.GetStates();
        currentIndex = 0;
    }

    void Update()
    {
        if (currentIndex < times.Count)
        {
            float t = times[currentIndex];
            Vector4 state = states[currentIndex];

            float body_x = t * scale_x;
            float body_y = 180 - state.x * scale_y;
            float wheel_x = t * scale_x;
            float wheel_y = 200 - state.z * scale_y;

            body.transform.position = new Vector3(body_x, body_y, 0);
            wheel.transform.position = new Vector3(wheel_x, wheel_y, 0);

            currentIndex++;
        }
    }
}

