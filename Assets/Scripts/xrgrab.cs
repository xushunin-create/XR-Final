using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class XRGrab : MonoBehaviour
{
    private float launchForce = 5;
    private float raycastDist = 50;
    private float grabRaduis = .2f;
    Collider[] hitColliders = new Collider[1];

    public Transform holdPoint;
    public LayerMask grabbableLayer;

    private Transform hoverObject = null;
    public Material glowMat;
    Material baseMat;

    private Transform heldObject = null;
    private Rigidbody heldRigidbody = null;
    public XRNode handRole = XRNode.LeftHand;
    bool gripState = false;

    LineRenderer line; // ← 新增

    void Start()
    {
        line = GetComponent<LineRenderer>(); // ← 新增
    }

    void Update()
    {
        // ===== 可视化射线（新增）=====
        if (line != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + transform.forward * 5f);
        }
        // ===========================

        InputDevices.GetDeviceAtXRNode(handRole)
            .TryGetFeatureValue(CommonUsages.gripButton, out bool grip);

        if (grip && !gripState)
        {
            if (heldObject == null)
            {
                FarGrab();
            }
        }
        else if (!grip && gripState && heldObject != null)
        {
            LaunchObject();
        }

        gripState = grip;
    }

    void FarGrab()
    {
        if (Physics.Raycast(transform.position, transform.forward,
            out RaycastHit hit, raycastDist, grabbableLayer))
        {
            if (hit.transform.parent == null)
            {
                StartCoroutine(PickUpObject(hit.transform));
            }
        }
    }

    void CloseGrab()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position,
            grabRaduis, hitColliders, grabbableLayer) > 0)
        {
            if (hitColliders[0].transform.parent == null)
            {
                StartCoroutine(PickUpObject(hitColliders[0].transform));
            }
        }
    }

    IEnumerator PickUpObject(Transform _trans)
    {
        heldObject = _trans;
        heldRigidbody = heldObject.GetComponent<Rigidbody>();
        heldRigidbody.isKinematic = true;

        float t = 0;
        while (t < .5f)
        {
            heldRigidbody.MovePosition(
                Vector3.Lerp(heldRigidbody.position,
                holdPoint.position, t));

            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        SnapToHand();
    }

    void SnapToHand()
    {
        heldObject.position = holdPoint.position;
        heldObject.parent = holdPoint;
    }

    void LaunchObject()
    {
        StopAllCoroutines();
        SnapToHand();

        heldRigidbody.isKinematic = false;
        heldRigidbody.linearVelocity = Vector3.zero;
        heldRigidbody.AddForce(
            transform.forward * launchForce,
            ForceMode.VelocityChange);

        heldObject.parent = null;
        StartCoroutine(LetGo());
    }

    IEnumerator LetGo()
    {
        yield return new WaitForSeconds(.1f);
        heldObject = null;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position,
            transform.forward,
            out RaycastHit hit,
            raycastDist,
            grabbableLayer))
        {
            if (hoverObject != hit.transform)
            {
                if (hoverObject != null)
                {
                    hoverObject.GetComponent<Renderer>().material = baseMat;
                }

                hoverObject = hit.transform;
                baseMat = hoverObject.GetComponent<Renderer>().material;
                hoverObject.GetComponent<Renderer>().material = glowMat;
            }
        }
        else
        {
            if (hoverObject != null)
            {
                hoverObject.GetComponent<Renderer>().material = baseMat;
                hoverObject = null;
            }
        }
    }
}