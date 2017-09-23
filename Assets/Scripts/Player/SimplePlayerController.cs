using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class SimplePlayerController : MonoBehaviour, IPlayerDirectionDispatcher
{
    private Rigidbody mRigidbody;
    private Animator[] mAnimators;

    private bool mFacingRight = true;
    public bool FacingRight
    {
        get { return mFacingRight; }
        set
        {
            mFacingRight = value;
            if (null != PlayerDirectionChanged)
            {
                PlayerDirectionChanged(this, new PlayerDirectionEventArgs(value));
            }
        }
    }

    public Transform RenderTransform;

    public float MoveForce = 365f;
    public float MaxSpeed = 5f;
    public float JumpForce = 1000f;
    public float RotationSpeed = 3.0f;
    public string GroundLayerName = "Ground";

    private bool mJump = false;
    private LayerMask mGroundLayer;

    public event PlayerDirectionEventHandler PlayerDirectionChanged;

    [SerializeField]
    private bool mGrounded = false;
    private Quaternion mRotatedQuaternion = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    private float mRotationProgress = 0.0f;

    private void Awake()
    {
        PrepareRigidbody();
        PrepareAnimation();
        mGroundLayer = LayerMask.NameToLayer(GroundLayerName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == mGroundLayer)
        {
            mGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == mGroundLayer)
        {
            mGrounded = false;
        }
    }

    private void PrepareRigidbody()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void PrepareAnimation()
    {
        mAnimators = GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        if (null != RenderTransform)
        {
            mRotationProgress = Mathf.Clamp(mRotationProgress + (Time.deltaTime * (mFacingRight ? RotationSpeed : -RotationSpeed)), 0.0f, 1.0f);
            RenderTransform.rotation = Quaternion.Lerp(Quaternion.identity, mRotatedQuaternion, mRotationProgress);
        }

        if (Input.GetButtonDown("Jump") && mGrounded)
        {
            mJump = true;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        if (null != mAnimators)
        {
            foreach (Animator anim in mAnimators)
            {
                anim.SetFloat("Speed", Mathf.Abs(h));
            }
        }

        if (h * mRigidbody.velocity.x < MaxSpeed)
            mRigidbody.AddForce(Vector3.right * h * MoveForce);

        if (Mathf.Abs(mRigidbody.velocity.x) > MaxSpeed)
            mRigidbody.velocity = new Vector3(Mathf.Sign(mRigidbody.velocity.x) * MaxSpeed, mRigidbody.velocity.y);

        if (h > 0 && !mFacingRight)
            Flip();
        else if (h < 0 && mFacingRight)
            Flip();

        if (mJump)
        {
            if (null != mAnimators)
            {
                foreach (Animator anim in mAnimators)
                {
                    anim.SetTrigger("Jump");
                }
            }
            mRigidbody.AddForce(new Vector3(0f, JumpForce, 0f));
            mJump = false;
        }
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
    }
}