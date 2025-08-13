using UnityEngine;

public class Animationsss : MonoBehaviour
{
    public CharacterController Controlador;
    public Animator Animation;

    public float Velocidad = 5f;
    public float Gravedad = -10f;
    public float Saltar = 3f;
    bool enMovimiento;

    public Transform EnElPiso;
    public float DistaciaDelPiso;
    public LayerMask MascaraDelPiso;



    public Vector3 VelocidadAbajo;
    public bool EstaEnElPiso;

    void Start()
    {
        Animation= GetComponent<Animator>();
    }


    void Update()
    {
        EstaEnElPiso = Physics.CheckSphere(EnElPiso.position, DistaciaDelPiso, MascaraDelPiso);

        if (EstaEnElPiso && VelocidadAbajo.y < 0)
        {
            VelocidadAbajo.y = -2;
           

        }
      


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float valorHorizontal = x + z;
        

        Vector3 mover = transform.right * x + transform.forward * z;
        Controlador.Move(mover * Velocidad * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && EstaEnElPiso)
        {
            VelocidadAbajo.y = Mathf.Sqrt(Saltar * -2f * Gravedad);
            
        }

        VelocidadAbajo.y += Gravedad * Time.deltaTime;
        Controlador.Move(VelocidadAbajo * Time.deltaTime);

        if (valorHorizontal <= 0)
        {
            valorHorizontal = valorHorizontal * -1;
            enMovimiento = false;
        }
        if (valorHorizontal > 0)
        {
            enMovimiento = true;
        }
        
        Animation.SetFloat("Vvelocity", VelocidadAbajo.y);
        Animation.SetBool("IsGrounded", EstaEnElPiso);
        Animation.SetBool("EnMovimiento", enMovimiento);
    }
}
