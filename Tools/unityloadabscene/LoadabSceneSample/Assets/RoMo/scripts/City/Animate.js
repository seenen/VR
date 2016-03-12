#pragma strict


// Drag this to the Water Object for more freedom of animation 


//----------------------------------------------------


// Declare here the input properties for each 
// shader properties you want to animate

// MainTex slot
var texture1Speed : Vector2;
var texture2Speed : Vector2;
var bumpMap1Speed : Vector2;
var bumpMap2Speed : Vector2;
var distortSpeed : Vector2;
var distort1Speed : Vector2;
var distort2Speed : Vector2;

private var texture1UV : Vector2;
private var texture2UV : Vector2;
private var bumpMap1UV : Vector2;
private var bumpMap2UV : Vector2;
private var distortUV : Vector2;
private var distort1UV : Vector2;
private var distort2UV : Vector2;


function Start () {

 
 //texture1Speed = texture1Speed /10;
 //texture2Speed = texture2Speed /10;
 //bumpMap1Speed = bumpMap1Speed /10;
 //bumpMap2Speed = bumpMap2Speed /10;
 //distortSpeed = distort1Speed /10;
 //distort1Speed = distort1Speed /10;
 //distort2Speed = distort2Speed /10; 

}

function Update () {

// Declare here the UV properties for each
// shader properties you're animating

//MainTex UV -- Repeat it for each shader properties
var time = Time.time / 10;
texture1UV.x = time * texture1Speed.x;
texture1UV.y = time * texture1Speed.y;

texture2UV.x = time * texture2Speed.x;
texture2UV.y = time * texture2Speed.y;

bumpMap1UV.x = time * bumpMap1Speed.x;
bumpMap1UV.y = time * bumpMap1Speed.y;

bumpMap2UV.x = time * bumpMap2Speed.x;
bumpMap2UV.y = time * bumpMap2Speed.y;

distortUV.x = time * distort1Speed.x;
distortUV.y = time * distort1Speed.y;

distort1UV.x = time * distort1Speed.x;
distort1UV.y = time * distort1Speed.y;

distort2UV.x = (time * distort2Speed.x);
distort2UV.y = (time * distort2Speed.y);




// For each property copy this line and chage texture names and UV properties
GetComponent.<Renderer>().material.SetTextureOffset("_Texture1", texture1UV);
GetComponent.<Renderer>().material.SetTextureOffset("_Texture2", texture2UV);
GetComponent.<Renderer>().material.SetTextureOffset("_BumpMap1", bumpMap1UV);
GetComponent.<Renderer>().material.SetTextureOffset("_BumpMap2", bumpMap2UV);
GetComponent.<Renderer>().material.SetTextureOffset("_DistortionMap1", distort1UV);
GetComponent.<Renderer>().material.SetTextureOffset("_DistortionMap2", distort2UV);


}