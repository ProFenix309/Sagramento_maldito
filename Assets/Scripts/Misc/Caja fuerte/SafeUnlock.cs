using UnityEngine;

public class SafeUnlock : MonoBehaviour
{
    public GameObject dialObject;
    public int minimumDegres = 5;
    public int DialNumberAngles = 45;

    public int[] digitAngles = new int[] { 90, 135, 0 };
    public bool[] digitSet = new bool[3];
    public int[] digitPasses = new int[] { 3, 3, 2 };
    public bool[] digitLeftTurn = new bool[] {true,false,true};


    public int passCount;
    public bool passReset;
    public int currentDigit;

    public bool turningLeft;

    public bool unlocked;

    public int oldAngle;
    public int currentAngle;

    private void Update()
    {
         if (unlocked) return;



         //Reset if you turn the wrong direction
         if (digitLeftTurn[currentDigit] != turningLeft && oldAngle != currentAngle) 
         {
            passCount = 0;
            currentDigit = 0;
            for (int i = 0; i < digitSet.Length; i++) 
            {
                digitSet[i] = false;
            }
            Debug.Log("Unlock failed, try again");
            //*reset noise
         }

         //Reset the dial rotation back to zero at 360 degrees
         if (dialObject.transform.localEulerAngles.x > 360 || dialObject.transform.localEulerAngles.x < -360)
         {
            dialObject.transform.localEulerAngles = Vector3.zero;
         }

         //set currentAngle to dial rotation by the sensitivity
         currentAngle = (int)(Mathf.RoundToInt(dialObject.transform.localEulerAngles.x/ minimumDegres) * minimumDegres);

        //Reset pass so you only increment passCount once
        if (!passReset && currentAngle != digitAngles[currentDigit]) //when you turn the dial after making a pass
        {
            passReset = true;
        }

        //update dial number change

        if (currentAngle % DialNumberAngles == 0)  //if the current angle is a multiple of the angle of the numbers on the dial 
        {
            //if the currentAngle has changed to a diferent dial number and the oldAngle is not zero
            if (currentAngle < oldAngle) 
            {
                if (oldAngle != 0 && oldAngle != 360) 
                    turningLeft = false; // set turningLeft to false if the current angle has increased from the old angle
                oldAngle = Mathf.RoundToInt(currentAngle / DialNumberAngles) * DialNumberAngles; //update oldAngle to current dial number
            }
            if (currentAngle > oldAngle)
            {
                if (oldAngle != 0 && oldAngle != 360) 
                    turningLeft = true; //set turningLeft to true if the current angle has increased from the old angle
                oldAngle = Mathf.RoundToInt(currentAngle / DialNumberAngles) * DialNumberAngles; //update oldAngle to current dial number
            }
        }

        //Putting in the safecode

        if (currentAngle == digitAngles[currentDigit]) // if dial objects rotation angle is equal to the angle of the current digit
        {
            
            if (passReset)
            {
                passCount++; // increment pass count
                passReset = false;
            }

            if (passCount >= digitPasses[currentDigit]) // if we turned the dial enough times to move on
            {
                //increment to the next digit in the safecode
                Debug.Log("Digit " + (currentDigit) + " set"); // if you're not on the last digit
                digitSet[currentDigit] = true;
                if (currentDigit != digitAngles.Length) // if you're not on the last digit
                {
                    currentDigit++;
                    turningLeft = digitLeftTurn[currentDigit];
                    passCount = 0;
                }

                // *Click noise
            }

        }

    }

}
