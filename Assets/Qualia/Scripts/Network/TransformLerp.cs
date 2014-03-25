using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
public class TransformLerp : Photon.MonoBehaviour
{
	public bool SyncPosition = true;
	public bool SyncRotation = true;
	public bool SyncScale = true;
	
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
	private float fractionPos;
	
	private Quaternion latestCorrectRot;
	private Quaternion onUpdateRot;
	private float fractionRot;
	
	private Vector3 latestCorrectScale;
	private Vector3 onUpdateScale;
	private float fractionScale;


    public void Awake()
    {
        if (photonView.isMine)
        {
            this.enabled = false;   // due to this, Update() is not called on the owner client.
        }
        
		if(SyncPosition){
	        latestCorrectPos = transform.position;
			onUpdatePos = transform.position;
		}
		
		if(SyncRotation){
			latestCorrectRot = transform.rotation;
			onUpdateRot = transform.rotation;
		}
		
		if(SyncScale){
			latestCorrectRot = transform.rotation;
			onUpdateRot = transform.rotation;
		}
    }

    /// <summary>
    /// While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
    /// </summary>
    /// <remarks>
    /// The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
    /// should write into the stream. Others will receive the content written by the owner and can read it.
    /// 
    /// Note: Send only what you actually want to consume/use, too!
    /// Note: If the owner doesn't write something into the stream, PUN won't send anything.
    /// </remarks>
    /// <param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
    /// <param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			Vector3 scale = transform.localScale;
			
			if(SyncPosition){
				stream.Serialize(ref pos);
			}
			if(SyncRotation){
				stream.Serialize(ref rot);
			}
			if(SyncScale){
				stream.Serialize(ref scale);
			}
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			Vector3 scale = transform.localScale;
			
			if(SyncPosition){
				stream.Serialize(ref pos);
				
				latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
				onUpdatePos = transform.position;  // we interpolate from here to latestCorrectPos
				fractionPos = 0;                           // reset the fraction we alreay moved. see Update()
			}
			if(SyncRotation){
				stream.Serialize(ref rot);
				
				latestCorrectRot = rot;                 // save this to move towards it in FixedUpdate()
				onUpdateRot = transform.rotation;  // we interpolate from here to latestCorrectPos
				fractionRot = 0;                           // reset the fraction we alreay moved. see Update()
			}
			if(SyncScale){
				stream.Serialize(ref scale);
				
				latestCorrectScale = scale;                 // save this to move towards it in FixedUpdate()
				onUpdateScale = transform.localScale;  // we interpolate from here to latestCorrectPos
				fractionScale = 0;                           // reset the fraction we alreay moved. see Update()
			}
			
        }
    }

    public void Update()
    {
        // We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
        // Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
        // Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
        //
        // Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
        // We want it to take a bit longer, so we multiply with 9 instead.
		
		if(SyncPosition){
	        fractionPos = fractionPos + Time.deltaTime * 9;
			transform.position = Vector3.Lerp(onUpdatePos, latestCorrectPos, fractionPos);    // set our pos between A and B
		}
			
		if(SyncRotation){
			fractionRot = fractionRot + Time.deltaTime * 9;
			transform.rotation = Quaternion.Lerp(onUpdateRot, latestCorrectRot, fractionRot);    // set our pos between A and B
		}
				
		if(SyncScale){
			fractionScale = fractionScale + Time.deltaTime * 9;
			transform.localScale = Vector3.Lerp(onUpdateScale, latestCorrectScale, fractionScale);    // set our pos between A and B
		}
    }
}
