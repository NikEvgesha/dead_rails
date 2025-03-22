using UnityEngine;

public class TrainController : MonoBehaviour
{
    [Header("Настройки поезда")]
    [Tooltip("Текущее количество топлива")]
    public float currentFuel = 100f;
    [Tooltip("Скорость расхода топлива (на единицу времени) при движении")]
    public float fuelConsumptionRate = 1f;
    [Tooltip("Ускорение поезда")]
    public float acceleration = 5f;
    [Tooltip("Замедление поезда")]
    public float deceleration = 5f;
    [Tooltip("Максимальная скорость поезда")]
    public float maxSpeed = 20f;

    [Header("Настройки водителя")]
    [Tooltip("Точка, где находится водитель (можно использовать для позиционирования камеры или игрока)")]
    public Transform driverSeat;
    [Tooltip("Находится ли игрок на водительском месте")]
    public bool playerOnSeat = false;

    // Текущая скорость поезда (положительная – вперёд, отрицательная – назад)
    private float currentSpeed = 0f;

    // Ссылка на Rigidbody для управления физикой
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Управление будет работать только если игрок находится за рулём
        if (!playerOnSeat)
            return;

        // Получаем ввод (по умолчанию "Vertical" — клавиши W/S или Up/Down)
        float input = Input.GetAxis("Vertical");

        if (currentFuel > 0)
        {
            if (input > 0)
            {
                // Ускоряем вперёд
                currentSpeed += acceleration * input * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
                ConsumeFuel(fuelConsumptionRate * Time.deltaTime);
            }
            else if (input < 0)
            {
                // Замедляем, а затем движемся назад
                currentSpeed -= deceleration * Mathf.Abs(input) * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
                ConsumeFuel(fuelConsumptionRate * Time.deltaTime);
            }
            else
            {
                // Если нет ввода — применяется замедление (фрикция)
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
            }
        }
        else
        {
            // Если топлива нет — поезд замедляется до 0
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Обновляем движение поезда (движение вдоль локальной оси вперед)
        Vector3 velocity = transform.right * currentSpeed;
        rb.velocity = velocity;
    }

    // Метод для расхода топлива
    void ConsumeFuel(float amount)
    {
        currentFuel -= amount;
        if (currentFuel < 0)
            currentFuel = 0;
    }

    // Метод для добавления топлива (вызывается из FuelDeposit)
    public void AddFuel(float amount)
    {
        currentFuel += amount;
        // При желании можно добавить ограничение максимального топлива
        // currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
    }
}
