using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{

    public class UGUI_DrawCardPanel : BasePanel
    {
        public Transform ExternalWheel;
        public Transform InternalWheel;

        private int targetVal1;
        private int targetVal2;
        private int exSpeed;
        private int inSpeed;
        private float waitTime;


        public void SingleDraw(int targetVal1, int targetVal2, Vector3 exSpeed, Vector3 inSpeed, float waitTime)
        {
            StopAllCoroutines();
            StartCoroutine(IE_SingleDrawProcess(targetVal1, targetVal2, exSpeed, inSpeed, waitTime));
        }

        IEnumerator IE_SingleDrawProcess(int targetVal1, int targetVal2, Vector3 exSpeed, Vector3 inSpeed, float waitTime)
        {
            float timer = 0;
            float interval = 0.01f;
            while (timer <= waitTime)
            {
                yield return new WaitForSeconds(interval);
                timer += interval;
                ExternalWheel.Rotate(exSpeed * interval);
                InternalWheel.Rotate(inSpeed * interval);
            }
            Quaternion val1 = Quaternion.Euler(new Vector3(0, 0, 36 * targetVal1));
            Quaternion val2 = Quaternion.Euler(new Vector3(0, 0, 36 * targetVal2));
                
            while (true)
            {
                if (InternalWheel.rotation == val2 && ExternalWheel.rotation == val1) break;
                yield return new WaitForSeconds(interval);
                ExternalWheel.rotation = Quaternion.Lerp(ExternalWheel.rotation, val1, 0.1f);
                InternalWheel.rotation = Quaternion.Lerp(InternalWheel.rotation, val2, 0.1f);
            }
            yield return null;
        }
    }

}