using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmoothFollowCamera : MonoBehaviour
{
    public string TargetTag = "Player";

    [HideInInspector]
    [SerializeField]
    private GameObject mTargetGameObject = null;
    [ExposeProperty]
    public GameObject TargetGameObject
    {
        get { return mTargetGameObject; }
        set
        {
            mTargetGameObject = value;
            if (null != value)
            {
                mTargetTransform = value.transform;
            }
            else
            {
                mTargetTransform = null;
            }
        }
    }

    [HideInInspector]
    [SerializeField]
    private bool mPlayerDirection = true;
    [ExposeProperty]
    public bool PlayerDirection
    {
        get { return mPlayerDirection; }
        set
        {
            if (mPlayerDirection != value)
            {
                Offset.x *= -1.0f;
            }
            mPlayerDirection = value;
        }
    }

    [Range(1.0f, 10.0f)]
    public float SmoothValue = 3.0f;
    public float CameraDirectionSwitchSpeed = 2.0f;
    public Vector3 DistanceFromTarget = new Vector3(0, 3.0f, -10.0f);
    public Vector3 Distance2DFromTarget = new Vector3(0, 3.0f, -10.0f);
    public Vector3 Offset = new Vector3(2.0f, 2.0f, 0.0f);

    [HideInInspector]
    [SerializeField]
    private Transform mTargetTransform = null;
    private Vector3 mTargetPosition;
    private Vector3 mCentralizedPosition;
    private Vector3 mTargetOffset;
    private Vector3 mDistanceFromTarget;

    private IPlayerDirectionDispatcher mPlayerDirectionDispatcher;

    private Transform mTransform;

    private void Start()
    {
        PrepareScript();
    }

    private void OnEnable()
    {
        PrepareScript();
    }

    private void PrepareScript()
    {
        if (null == TargetGameObject)
        {
            TargetGameObject = GameObject.FindGameObjectWithTag(TargetTag);
            if (null == TargetGameObject)
            {
                this.enabled = false;
                return;
            }
        }

        mPlayerDirectionDispatcher = (IPlayerDirectionDispatcher)TargetGameObject.GetComponent(typeof(IPlayerDirectionDispatcher));

        if (null != mPlayerDirectionDispatcher)
        {
            mPlayerDirectionDispatcher.PlayerDirectionChanged += OnPlayerDirectionChanged;
        }

        if (null == mTransform)
        {
            mTransform = transform;
        }

        mDistanceFromTarget = DistanceFromTarget;

        mTargetOffset = Offset;

        mCentralizedPosition = GetTargetPosition();
        mTransform.position = mCentralizedPosition;
        mTransform.LookAt(mTargetPosition);
        mTransform.Translate(mTargetOffset);
    }

    private void OnPlayerDirectionChanged(IPlayerDirectionDispatcher sender, PlayerDirectionEventArgs e)
    {
        PlayerDirection = e.PlayerFacingRight;
    }

    private void Update()
    {
        mTargetPosition = GetTargetPosition();
        mCentralizedPosition += (mTargetPosition - mCentralizedPosition) * Time.deltaTime * SmoothValue;

        mTransform.position = mCentralizedPosition;

        mTransform.LookAt(mTargetTransform);

        mTargetOffset += (Offset - mTargetOffset) * Time.deltaTime * CameraDirectionSwitchSpeed;

        mTransform.Translate(mTargetOffset);
    }

    private Vector3 GetTargetPosition()
    {
        return mTargetTransform.position + (mTargetTransform.right * mDistanceFromTarget.x) + (mTargetTransform.up * mDistanceFromTarget.y) + (mTargetTransform.forward * mDistanceFromTarget.z);
    }

    private void OnDestroy()
    {
        if (null != mPlayerDirectionDispatcher)
        {
            mPlayerDirectionDispatcher.PlayerDirectionChanged -= OnPlayerDirectionChanged;
        }
    }

    private void On2DSceneEnter()
    {
        mDistanceFromTarget = Distance2DFromTarget;
    }

    private void On2DSceneExit()
    {
        mDistanceFromTarget = DistanceFromTarget;
    }
}