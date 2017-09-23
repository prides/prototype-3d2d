using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraProjectionChange : MonoBehaviour
{
    public float ProjectionChangeTime = 0.5f;
    public bool ChangeProjection = false;

    private bool _changing = false;
    private float _currentT = 0.0f;

    private Camera mCamera = null;

    private void Awake()
    {
        if (null == mCamera)
        {
            mCamera = this.gameObject.GetComponent<Camera>();
        }
    }

    private void Update()
    {
        if (_changing)
        {
            ChangeProjection = false;
        }
        else if (ChangeProjection)
        {
            _changing = true;
            _currentT = 0.0f;
        }
        if (Input.GetButtonDown("ChangeView"))
        {
            ChangeProjection = true;
        }
    }

    private void LateUpdate()
    {
        if (!_changing)
        {
            return;
        }

        var currentlyOrthographic = mCamera.orthographic;
        Matrix4x4 orthoMat, persMat;
        if (currentlyOrthographic)
        {
            orthoMat = mCamera.projectionMatrix;

            mCamera.orthographic = false;
            mCamera.ResetProjectionMatrix();
            persMat = mCamera.projectionMatrix;
        }
        else
        {
            persMat = mCamera.projectionMatrix;

            mCamera.orthographic = true;
            mCamera.ResetProjectionMatrix();
            orthoMat = mCamera.projectionMatrix;
        }
        mCamera.orthographic = currentlyOrthographic;

        _currentT += (Time.deltaTime / ProjectionChangeTime);
        if (_currentT < 1.0f)
        {
            if (currentlyOrthographic)
            {
                mCamera.projectionMatrix = MatrixLerp(orthoMat, persMat, _currentT * _currentT);
            }
            else
            {
                mCamera.projectionMatrix = MatrixLerp(persMat, orthoMat, Mathf.Sqrt(_currentT));
            }
        }
        else
        {
            _changing = false;
            mCamera.orthographic = !currentlyOrthographic;
            mCamera.ResetProjectionMatrix();
        }
    }

    private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        var newMatrix = new Matrix4x4();
        newMatrix.SetRow(0, Vector4.Lerp(from.GetRow(0), to.GetRow(0), t));
        newMatrix.SetRow(1, Vector4.Lerp(from.GetRow(1), to.GetRow(1), t));
        newMatrix.SetRow(2, Vector4.Lerp(from.GetRow(2), to.GetRow(2), t));
        newMatrix.SetRow(3, Vector4.Lerp(from.GetRow(3), to.GetRow(3), t));
        return newMatrix;
    }

    private void On2DSceneEnter()
    {
        ChangeProjection = true;
    }

    private void On2DSceneExit()
    {
        ChangeProjection = true;
    }
}