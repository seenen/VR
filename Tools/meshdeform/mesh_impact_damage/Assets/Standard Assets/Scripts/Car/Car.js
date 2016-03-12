#pragma strict
// Approximate motor torque curve with a simple parabola:
// torque goes from motorMinTorque at zero RPM to motorMaxTorque at
// motorRpmRange/2 RPM then back to motorMinTorque at motorRpmRange.
// This is way off the real motors, but works good for a simple car.
var motorMinTorque = 300.0;
var motorMaxTorque = 475.0;
var motorRpmRange = 6000.0;

// Brake and handbrake torques.
var brakeStrength = 100.0;
var handBrakeStrength = 500.0;

// Steering: wheels can turn up to lowSpeedSteerAngle when car is still;
// and up to highSpeedSteerAngle when car goes at highSpeed km/h.
// This is to decrease steering at high velocities so that playing with
// plain keyboard is possible.
var lowSpeedSteerAngle = 45.0;
var highSpeedSteerAngle = 10.0;
var highSpeed = 100.0;

// How much we move the mass center vertically.
var massCenterY = -0.1;

// How much velocity based down-pressure force we apply.
var downPressureFactor = 0.5;

// other motor parameters
// defaults for corvette
//var rpmToGearUp = 5000.0;
//var rpmToGearDown = 2500.0;
private var minRpm = 800.0;

// In this project we don't simulate transmission (I ran out of time!).
// Just use 1.0 gear ratio.
//var forwardGearRatios = [2.66, 1.78, 1.30, 1.00, 0.74, 0.50];
//var backwardGearRatio = -2.90;
private var gearRatio = 1.0;

var differentialRatio = 3.42;

// UI indicators
var uiSpeed : GUIText;
var uiMotorRpm : GUIText;

// All Wheel components down the hierarchy
private var wheels : Component[];


private var curMotorRpm : float = 0.0;

// ------------------------------------------------------------

function Start()
{
	// get all wheels
	wheels = GetComponentsInChildren(Wheel);
	// lower center of mass
	GetComponent.<Rigidbody>().centerOfMass = Vector3 (0.0, massCenterY, 0.0);
}


function FixedUpdate()
{
	// calculate current speed in km/h
	var kmPerH = GetComponent.<Rigidbody>().velocity.magnitude * 3600*0.001;
	
	// space does handbrake
	var handbrake = 0.0;
	if( Input.GetKey("space") )
		handbrake = handBrakeStrength;
	
	// current wheels rpm
	var wheelsRpm = ComputeRpmFromWheels();
	
	// find maximum steer angle (dependent on car velocity)
	var maxSteer = Mathf.Lerp( lowSpeedSteerAngle, highSpeedSteerAngle, kmPerH / highSpeed );
	
	// steering
	var steerAmount = Input.GetAxis ("Horizontal");
	var steer = steerAmount * maxSteer;
	
	// apply down pressure to the car (only if any wheel is grounded)
	var isGrounded = false;
	for( var w:Wheel in wheels )
	{
		var wc : WheelCollider = w.GetComponent.<Collider>();
		if( wc.isGrounded )
		{
			isGrounded = true;
			break;
		}
	}
	if( isGrounded )
	{		
		var downPressure = Vector3(0,0,0);
		downPressure.y = -Mathf.Pow(GetComponent.<Rigidbody>().velocity.magnitude, 1.2) * downPressureFactor;
		downPressure.y = Mathf.Max( downPressure.y, -70 );
		GetComponent.<Rigidbody>().AddForce( downPressure, ForceMode.Acceleration );
	}
	
	// motor & brake
	var motor = 0.0;
	var brake = 0.0;
	var motorAxis = Input.GetAxis("Vertical");
	var axisTorque = ComputeAxisTorque( Mathf.Abs(motorAxis) );
	if( Mathf.Abs(motorAxis) > 0.1 ) // if any of up/down keys is pressed
	{
		if( wheelsRpm * motorAxis < 0.0 )
		{
			// user is trying to drive in the opposite direction - treat that as brake
			brake = brakeStrength;
		}
		else
		{
			// user is driving in the same direction - treat as motor
			motor = motorAxis * axisTorque;
		}
	}


	// update all wheels	
	for( var w:Wheel in wheels )
	{
		w.UpdateWheel( handbrake, motor, brake, steer );
	}
	
	// update UI texts if we have them
	if( uiSpeed != null )
		uiSpeed.text = kmPerH.ToString("f1") + "\tkm/h";
	if( uiMotorRpm != null )
		uiMotorRpm.text = (minRpm+curMotorRpm).ToString("f0") + "\trpm";
		
	GetComponent.<AudioSource>().pitch = 0.8 + (curMotorRpm/motorRpmRange) * 0.8;
}

// ------------------------------------------------------------


// A simple parabola to approximate the shape of motor torque
// curve. In real cars you probably would do this with explicit
// table that maps rpm to torque.
function MotorTorqueCurve( motorrpm : float )
{
	var x = (motorrpm/motorRpmRange) * 2 - 1;
	var y = 1 - x * x;
	
	// x == 1 means engine runs atm max. RPM - dont get faster anymore
	if(x==1)
		return 0;
	else
		return Mathf.Lerp( motorMinTorque, motorMaxTorque, y );
}


// Given acceleration pedal input, computes the torque that
// we want to apply to wheel axles.
function ComputeAxisTorque( accelPedal : float ) : float
{
	// Compute current motor rpm, based on wheels rpm
	var wheelRpm = Mathf.Abs( ComputeRpmFromWheels() );
	ComputeMotorRpm( wheelRpm );
	
	// Figure out motor torque based on motor rpm
	var motorTorque = accelPedal * MotorTorqueCurve( curMotorRpm );
	
	// Compute wheel axle torque from motor torque
	return motorTorque / gearRatio / differentialRatio;
}


// Compute motor RPM from wheel RPM
function ComputeMotorRpm( rpm : float )
{
	curMotorRpm = rpm * gearRatio * differentialRatio;
	curMotorRpm = Mathf.Min( curMotorRpm, motorRpmRange );
}


// Compute average RPM of all motorized wheels
function ComputeRpmFromWheels() : float
{
	var rpm : float = 0.0;
	var count : int = 0;
	for( var w:Wheel in wheels )
	{
		if( w.motorised )
		{
			var wc : WheelCollider = w.GetComponent.<Collider>();
			rpm += wc.rpm;
			++count;
		}
	}
	if( count != 0 )
		rpm /= count;
		
	return rpm;
}
