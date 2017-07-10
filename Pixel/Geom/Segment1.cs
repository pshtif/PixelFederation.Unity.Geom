using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixel.Geom
{
    public class Segment1
    {
        public float end;
        public float strength;

        public Segment1(float p_end, float p_strength) {
            end = p_end;
            strength = p_strength;
        }

        virtual public float Calculate(float p_start, float p_delta)
        {
            return 0;
        }
    }

    public class LinearSegment1 : Segment1
    {
        public LinearSegment1(float p_end, float p_strength) : base(p_end, p_strength) {
        }

        override public float Calculate(float p_start, float p_delta) {
            return p_start + p_delta * (end - p_start);
        }
    }

    public class QuadraticBezierSegment1 : Segment1
    {
        public float control;

        public QuadraticBezierSegment1(float p_end, float p_strength, float p_control) : base (p_end, p_strength) {
            control = p_control;
        }


       override public float Calculate(float p_start, float p_delta) {
            float inv = (1 - p_delta);
            return inv* inv * p_start + 2 * inv * p_delta * control + p_delta* p_delta * end;
        }
    }

    class CubicBezierSegment1 : Segment1
    {
        public float control1;
        public float control2;

        public CubicBezierSegment1(float p_end, float p_strength, float p_control1, float p_control2) : base(p_end, p_strength) {
            control1 = p_control1;
            control2 = p_control2;
        }


        override public float Calculate(float p_start, float p_delta) {
            float inv = (1 - p_delta);
            float inv2 = inv * inv;
            float d2 = p_delta * p_delta;
            return inv2 * inv * p_start + 3 * inv2 * p_delta * control1 + 3 * inv * d2 * control2 + d2 * p_delta * end;
        }
    }
}
