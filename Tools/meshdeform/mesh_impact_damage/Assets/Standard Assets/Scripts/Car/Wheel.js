#pragma strict

var steerable = false;
var motorised = true;
var affectedByHandbrake = true;

// The graphical object that is rolled and turned by this wheel
var wheelObject : Transform;

private var originalRotation : Quaternion;
private var accumSpinRotation : Quaternion;
private var motorBrake : float = 20.0;

function Start()
{
	if( wheelObject ) {
		originalRotation = wheelObject.transform.localRotation;
		accumSpinRotation = Quaternion.identity;
	}
}


// UpdateWheel is called from the main Car script
function UpdateWheel( handbrake : float, motor : float, brake : float, steer : float )
{
	var wc : WheelCollider = GetComponent.<Collider>();
	// apply motor torque for motorised wheels
	if( motorised )
	{
		wc.motorTorque = motor;
	}
		
	// apply brake or handbrake, depending on which is larger
	if( affectedByHandbrake && handbrake > brake )
		brake = handbrake;
		
	wc.brakeTorque = brake;
	
	if(motor < 1.0 && motor > -1.0)
		wc.brakeTorque+=motorBrake;	
		
	// for steerable wheels, steer it and turn the wheelObject if we have one
	if( steerable )
	{
		wc.steerAngle = steer;
		if( wheelObject != null ) {
			wheelObject.transform.localRotation = originalRotation * Quaternion.Euler(0, steer,0);
		}
	}

	// roll the rendered wheel object if we have one
	if( wheelObject != null ) {
		// RPM is 1 for one full rotation per minute.
		// Convert that to degrees per deltaTime.
		var rotation = wc.rpm / 60.0 * 360.0 * Time.deltaTime;
		accumSpinRotation = accumSpinRotation * Quaternion.Euler( rotation, 0, 0 );
		wheelObject.transform.localRotation *= accumSpinRotation;
	}
}
