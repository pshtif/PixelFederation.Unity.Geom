/*
 *  Curve library for Unity
 * 
 *	Copyright 2011-2017 Peter @sHTiF Stefcek. All rights reserved.
 *
 *	Ported from Genome2D framework (https://github.com/pshtif/Genome2D/)
 *	
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixel.Geom
{
    public class Curve1
    {
        public float start;

        private List<Segment1> _segments;
        private int _pathLength;
        private float _totalStrength;

        public Curve1(float p_start = 0) {
            start = p_start;
            _segments = new List<Segment1>();
            _pathLength = 0;
            _totalStrength = 0;
        }

        public bool IsConstant() {
            return _pathLength == 0;
        }

        private void AddSegment(Segment1 p_segment) {
            _segments.Add(p_segment);
            _totalStrength += p_segment.strength;
            _pathLength++;
        }

        public void Clear() {
            _pathLength = 0;
            _segments = new List<Segment1>();
            _totalStrength = 0;
        }

        public Curve1 Line(float p_end, float p_strength = 1) {
            AddSegment(new LinearSegment1(p_end, p_strength));

            return this;
        }
        /*
        public float GetEnd() {
            if (_pathLength == 0) throw new System.Exception("This curve has no points and therefore no end.");
            return _segments[_pathLength - 1].GetEnd();
        }
        /**/

        public float Calculate(float p_delta) {
            float r = start;
            if (_pathLength == 1) {
                r = _segments[0].Calculate(start, p_delta);
            } else if (_pathLength > 1) {
                float ratio = p_delta * _totalStrength;
                float lastEnd = start;

                for (int i = 0; i<_pathLength; ++i) {
                    Segment1 path = _segments[i];
                    if (ratio > path.strength) {
                        ratio -= path.strength;
                        lastEnd = path.end;
                    } else {
                        r = path.Calculate(lastEnd, ratio / path.strength);
                    }
                }
            }

            return r;
        }

        static public Curve1 CreateLine(float p_end, float p_strength = 1) {
            return new Curve1().Line(p_end, p_strength);
        }

        public Curve1 QuadraticBezier(float p_end, float p_control, float p_strength = 1) {
            AddSegment(new QuadraticBezierSegment1(p_end, p_strength, p_control));
            return this;
        }

        public Curve1 CubicBezier(float p_end, float p_control1, float p_control2, float p_strength = 1) {
            AddSegment(new CubicBezierSegment1(p_end, p_strength, p_control1, p_control2));
            return this;
        }
    }
}
