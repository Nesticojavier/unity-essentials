using UnityEngine;

public class KinematicWander2 : MonoBehaviour
{
    public float maxSpeed = 5f;         // Velocidad máxima
    public float maxRotation = 45f;     // Máxima rotación por frame (en grados)
    private Rigidbody2D rb2D;           // El Rigidbody2D del personaje
    private float currentOrientation;   // Orientación actual del personaje (en grados)

    // Start es llamado antes del primer frame
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();  // Obtener el Rigidbody2D del objeto
        currentOrientation = transform.eulerAngles.z;  // Obtener la orientación inicial del objeto
    }

    // Update es llamado una vez por frame
    void Update()
    {
        // 1. Obtener la velocidad según la orientación actual
        Vector2 velocity = OrientationToVector(currentOrientation) * maxSpeed;

        // 2. Aplicar la velocidad al Rigidbody2D
        rb2D.velocity = velocity;

        // 3. Generar una rotación aleatoria dentro del rango permitido
        float randomRotation = RandomBinomial() * maxRotation;

        // 4. Actualizar la orientación del personaje con la rotación aleatoria
        currentOrientation += randomRotation;

        // 5. Aplicar la rotación al Rigidbody2D (si quieres que rote visualmente)
        rb2D.rotation = currentOrientation;
    }

    // Método para convertir la orientación (ángulo en grados) en un vector 2D
    Vector2 OrientationToVector(float orientation)
    {
        // Convertir los grados a radianes
        float radians = orientation * Mathf.Deg2Rad;

        // Usar trigonometría para obtener la dirección
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
    }

    // Método que devuelve un valor entre -1 y 1 de manera aleatoria (randomBinomial)
    float RandomBinomial()
    {
        // return Random.Range(-1f, 1f);
        System.Random random = new System.Random();
        return (float)(random.NextDouble() - random.NextDouble());
    }
}
