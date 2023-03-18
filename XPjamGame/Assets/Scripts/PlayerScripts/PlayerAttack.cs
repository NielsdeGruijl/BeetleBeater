using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform SwordAnchor;

    [SerializeField] private float slashDuration;
    [SerializeField] private float slashSpeed;

    private bool slashing = false;
    private bool canSlash = true;

    private Vector3 startRotation;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSlash)
        {
            //slashing = true;
            StartCoroutine(SwingSwordCo());
            canSlash = false;

            startRotation = SwordAnchor.localEulerAngles;
        }

        //if (slashing)
        //{
            
        //    if (SwordAnchor.rotation.eulerAngles.z >= -40)
        //    {
        //        SwordAnchor.Rotate(Vector3.forward * (slashSpeed * Time.deltaTime) * -1);
        //    }       
        //}
    }

    private IEnumerator SwingSwordCo()
    {
        float swung = 0f;
        while (swung < 80f)
        {
            SwordAnchor.Rotate(Vector3.forward * (slashSpeed * Time.deltaTime) * -1);
            swung += slashSpeed * Time.deltaTime;
            yield return null;
        }
        //reset sword
        SwordAnchor.localRotation = Quaternion.Euler(startRotation);
        canSlash = true;
    }

/*    IEnumerator SwingSword()
    {
        while(totalAmountRotated <= 80)
        {
            totalAmountRotated += 1;
            SwordAnchor.rotation = Quaternion.Euler(0, 0, startZRotation - totalAmountRotated);

            yield return null;
        }
    }*/
}
