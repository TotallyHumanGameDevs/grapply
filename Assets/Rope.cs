using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    public Transform pnt1;
    public Transform pnt2;
    public float length;
    public int segments;

    LineRenderer lineR;

    // Start is called before the first frame update
    void Start()
    {
        lineR = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pnt1.position.x > pnt2.position.x){
            Transform placeholder = pnt1;
            pnt1 = pnt2;
            pnt2 = placeholder;
        }

        float hyp = Mathf.Sqrt(Mathf.Pow(pnt1.position.x - pnt2.position.x, 2f) + Mathf.Pow(pnt1.position.y - pnt2.position.y, 2f));
        if(hyp + 0.001f >= length){
            lineR.positionCount = 2;
            lineR.SetPosition(0, pnt1.position);
            lineR.SetPosition(1, pnt2.position);
        }
        else if(pnt1.position.x == pnt2.position.x){
            lineR.positionCount = 2;
            if(pnt1.position.y > pnt2.position.y){
                lineR.SetPosition(0, pnt1.position);
                lineR.SetPosition(1, new Vector2(pnt2.position.x, pnt2.position.y - (length - pnt1.position.y + pnt2.position.y) / 2f));
            }
            else{
                lineR.SetPosition(0, pnt2.position);
                lineR.SetPosition(1, new Vector2(pnt1.position.x, pnt1.position.y - (length - pnt2.position.y + pnt1.position.y) / 2f));
            }
        }
        else{
            lineR.positionCount = segments + 1;

            float dx = pnt2.position.x - pnt1.position.x;
            float xb = (pnt2.position.x + pnt1.position.x) / 2f;

            float dy = pnt2.position.y - pnt1.position.y;
            float yb = (pnt2.position.y + pnt1.position.y) / 2f;

            float r = Mathf.Sqrt(Mathf.Pow(length, 2f)-Mathf.Pow(dy, 2f))/dx;

            float A = 0f;
            if(r < 3)
                A = Mathf.Sqrt(6 * (r - 1f));
            else
                A = Mathf.Log(2f * r) + Mathf.Log(Mathf.Log(2f * r));

            float left = r * A;
            float right = Sinh(A);
            
            for(int i = 0; i < 5 && Mathf.Abs(left - right) > 0.00001f; i++){
                A -= (Sinh(A) - r * A) / (Cosh(A) - r);

                left = r * A;
                right = Sinh(A);
            }

            float a = dx/(2 * A);
            float b = xb - a * ITanh(dy/length);
            float c = yb - (length/(2 * Tanh(A)));

            float x = pnt1.position.x;
            float segmentL = dx/((float)segments);
            for (int i = 0; i <= segments; i++){
                lineR.SetPosition(i, new Vector2(x, a * Cosh((x - b) / a) + c));
                x += segmentL;
            }
        }
    }

    public float Cosh(float z){
        return ((Mathf.Exp(z) + Mathf.Exp(-z)) / 2f);
    }
    public float Sinh(float z){
        return ((Mathf.Exp(z) - Mathf.Exp(-z)) / 2f);
    }
    public float Tanh(float z){
        return Sinh(z) / Cosh(z);
    }
    public float ITanh(float z){
        return (0.5f * Mathf.Log((1f + z) / (1f - z)));
    }
}
