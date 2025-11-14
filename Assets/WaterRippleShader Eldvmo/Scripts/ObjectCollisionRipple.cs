using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eldvmo.Ripples
{
    public class ObjectCollisionRipple : MonoBehaviour
    {
        private bool isInWater = false;
        [SerializeField] private MeshRenderer ripplePlane;
        private Collider ripplePlaneCollider;
        private Vector4[] ripplePoints = new Vector4[10];
        private int rippleIndex = 0;
        private Vector2 _oldInputCentre;
        private int waterLayerMask;
        [SerializeField] private Collider waterTrigger;

        [Header("Floating Settings")]
        [SerializeField] private bool isFloatingWithWater = true;
        [SerializeField] private float moveUpHeight = 2f;
        [SerializeField] private float buoyancyForce = 5f;

        [Header("Ripple Settings")]
        [SerializeField] private float rippleUpdateDistance = 0.05f;
        [SerializeField] private float rippleCheckInterval = 0.02f; // Cada cu�nto verifica posici�n

        private Rigidbody rb;
        private float nextRippleCheck = 0f;
        private Vector3 lastVelocity;

        void Start()
        {
            ripplePlaneCollider = ripplePlane.GetComponent<Collider>();
            waterLayerMask = LayerMask.GetMask("Water");
            rb = GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogWarning("No Rigidbody found. Adding one for water physics.");
                rb = gameObject.AddComponent<Rigidbody>();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger Enter: {other.gameObject.name}"); // LÍNEA DE DEBUG
            if (other == waterTrigger)
            {
                isInWater = true;
                Debug.Log("¡Entró al agua!"); // LÍNEA DE DEBUG
                // Generar onda al entrar al agua
                CreateRippleAtCurrentPosition(large: true);
                lastVelocity = rb.linearVelocity;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other == waterTrigger)
            {
                isInWater = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other == waterTrigger)
            {
                isInWater = false;
                // Onda al salir del agua
                CreateRippleAtCurrentPosition(large: true);
            }
        }

        void FixedUpdate()
        {
            if (!isInWater) return;

            // Aplicar flotaci�n
            if (isFloatingWithWater)
            {
                ApplyBuoyancy();
            }

            // Generar ondas mientras se mueve en el agua
            if (Time.time >= nextRippleCheck)
            {
                nextRippleCheck = Time.time + rippleCheckInterval;

                // Solo genera ondas si se est� moviendo
                if (rb.linearVelocity.magnitude > 0.1f)
                {
                    CreateRippleAtCurrentPosition(large: false);
                }
            }
        }

        private void CreateRippleAtCurrentPosition(bool large = false)
        {
            // Raycast desde arriba del objeto hacia el plano de agua
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Ray ray = new Ray(origin, Vector3.down);
            RaycastHit hit;

            Debug.DrawRay(origin, Vector3.down * 10f, Color.red, 1f);

            if (Physics.Raycast(ray, out hit, 5f, waterLayerMask))
            {
                Vector2 uv = hit.textureCoord;

                // Evitar ondas muy cercanas (spam)
                if (Vector2.Distance(_oldInputCentre, uv) < rippleUpdateDistance)
                    return;

                // Registrar nueva onda
                ripplePoints[rippleIndex] = new Vector4(uv.x, uv.y, Time.time, 0);
                rippleIndex = (rippleIndex + 1) % ripplePoints.Length;
                _oldInputCentre = uv;

                // Actualizar shader
                ripplePlane.material.SetVectorArray("_InputCentre", ripplePoints);
            }
        }

        private void ApplyBuoyancy()
        {
            // Raycast para encontrar la superficie del agua
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Ray ray = new Ray(origin, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f, waterLayerMask))
            {
                float waterSurfaceY = hit.point.y;
                float targetY = waterSurfaceY + moveUpHeight;
                float currentY = transform.position.y;

                // Aplicar fuerza de flotaci�n suave
                if (currentY < targetY)
                {
                    rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
                }

                // Reducir gravedad en el agua
                rb.linearDamping = 2f;
                rb.angularDamping = 2f;
            }
        }

        void OnDestroy()
        {
            // Limpiar ondas al destruir el objeto
            if (ripplePlane != null && ripplePlane.material != null)
            {
                for (int i = 0; i < ripplePoints.Length; i++)
                {
                    ripplePoints[i] = Vector4.zero;
                }
                ripplePlane.material.SetVectorArray("_InputCentre", ripplePoints);
            }
        }
    }
}