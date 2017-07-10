using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixel.Geom
{
    public class Segment3
    {
        public Vector3 end;
        public float strength;

        public Segment3(Vector3 p_end, float p_strength) {
            end = p_end;
            strength = p_strength;
        }

        virtual public Vector3 Calculate(Vector3 p_start, float p_delta)
        {
            return Vector3.zero;
        }
    }

    public class LinearSegment3 : Segment3
    {
        public LinearSegment3(Vector3 p_end, float p_strength) : base(p_end, p_strength) {
        }

        override public Vector3 Calculate(Vector3 p_start, float p_delta) {
            return p_start + p_delta * (end - p_start);
        }
    }

    public class QuadraticBezierSegment3 : Segment3
    {
        public Vector3 control;

        public QuadraticBezierSegment3(Vector3 p_end, float p_strength, Vector3 p_control) : base (p_end, p_strength) {
            control = p_control;
        }


       override public Vector3 Calculate(Vector3 p_start, float p_delta) {
            float inv = (1 - p_delta);
            return inv * inv * p_start + 2 * inv * p_delta * control + p_delta * p_delta * end;
        }
    }

    class CubicBezierSegment3 : Segment3
    {
        public Vector3 control1;
        public Vector3 control2;

        public CubicBezierSegment3(Vector3 p_end, float p_strength, Vector3 p_control1, Vector3 p_control2) : base(p_end, p_strength) {
            control1 = p_control1;
            control2 = p_control2;
        }


        override public Vector3 Calculate(Vector3 p_start, float p_delta) {
            float inv = (1 - p_delta);
            float inv2 = inv * inv;
            float d2 = p_delta * p_delta;
            return inv2 * inv * p_start + 3 * inv2 * p_delta * control1 + 3 * inv * d2 * control2 + d2 * p_delta * end;
        }
    }
}
