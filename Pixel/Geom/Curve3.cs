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
    public class Curve3
    {
        public Vector3 start;

        private List<Segment3> _segments;
        private int _pathLength;
        private float _totalStrength;

        public Curve3(Vector3 p_start = default(Vector3)) {
            start = p_start;
            _segments = new List<Segment3>();
            _pathLength = 0;
            _totalStrength = 0;
        }

        public bool IsConstant() {
            return _pathLength == 0;
        }

        private void AddSegment(Segment3 p_segment) {
            _segments.Add(p_segment);
            _totalStrength += p_segment.strength;
            _pathLength++;
        }

        public void Clear() {
            _pathLength = 0;
            _segments = new List<Segment3>();
            _totalStrength = 0;
        }

        public Curve3 Line(Vector3 p_end, float p_strength = 1) {
            AddSegment(new LinearSegment3(p_end, p_strength));

            return this;
        }
        /*
        public float GetEnd() {
            if (_pathLength == 0) throw new System.Exception("This curve has no points and therefore no end.");
            return _segments[_pathLength - 1].GetEnd();
        }
        /**/

        public Vector3 Calculate(float p_delta) {
            Vector3 r = start;
            if (_pathLength == 1) {
                r = _segments[0].Calculate(start, p_delta);
            } else if (_pathLength > 1) {
                float ratio = p_delta * _totalStrength;
                Vector3 lastEnd = start;

                for (int i = 0; i<_pathLength; ++i) {
                    Segment3 path = _segments[i];
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

        static public Curve3 CreateLine(Vector3 p_end, float p_strength = 1) {
            return new Curve3().Line(p_end, p_strength);
        }

        public Curve3 QuadraticBezier(Vector3 p_end, Vector3 p_control, float p_strength = 1) {
            AddSegment(new QuadraticBezierSegment3(p_end, p_strength, p_control));
            return this;
        }

        public Curve3 CubicBezier(Vector3 p_end, Vector3 p_control1, Vector3 p_control2, float p_strength = 1) {
            AddSegment(new CubicBezierSegment3(p_end, p_strength, p_control1, p_control2));
            return this;
        }
    }
}
