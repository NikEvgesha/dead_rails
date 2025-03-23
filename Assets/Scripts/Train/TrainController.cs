using UnityEngine;

public class TrainController : MonoBehaviour
{
    [Header("Настройки поезда")]
    [Tooltip("Текущее количество топлива")]
    public float currentFuel = 100f;
    [Tooltip("Коэффициент расхода топлива – расход топлива пропорционален текущей скорости")]
    public float fuelConsumptionRate = 0.1f;
    [Tooltip("Ускорение поезда")]
    public float acceleration = 5f;
    [Tooltip("Замедление поезда")]
    public float deceleration = 5f;
    [Tooltip("Максимальная скорость поезда")]
    public float maxSpeed = 20f;

    [Header("Настройки водителя")]
    [Tooltip("Находится ли игрок на водительском месте")]
    public bool playerOnSeat = false;

    // Текущая скорость (поезд может двигаться только вперёд)
    private float currentSpeed = 0f;

    // Считываемое значение ввода (ось "Vertical"), получаемое в Update и используемое в FixedUpdate
    private float inputValue;

    // Таймер для игнорирования остаточного ввода сразу после входа
    private float ignoreInputTime = 0f;
    [Tooltip("Длительность игнорирования ввода после входа в поезд (сек.)")]
    public float ignoreInputDuration = 0.5f;

    private void Awake()
    {
        // Если на объекте есть Rigidbody, переводим его в кинематический режим,
        // чтобы он не реагировал на гравитацию и столкновения
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if (!playerOnSeat)
            return;

        // Пока активен таймер игнорирования, ввод обнуляется
        if (ignoreInputTime > 0)
        {
            inputValue = 0;
        }
        else
        {
            // Получаем ввод по оси "Vertical"
            inputValue = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        if (!playerOnSeat)
            return;

        // Обновляем таймер игнорирования ввода
        if (ignoreInputTime > 0)
        {
            ignoreInputTime -= Time.fixedDeltaTime;
            inputValue = 0;
        }

        // Если топлива достаточно и ввод положительный – ускоряем поезд
        if (currentFuel > 0 && inputValue > 0)
        {
            currentSpeed += acceleration * inputValue * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            float consumption = fuelConsumptionRate * currentSpeed * Time.fixedDeltaTime;
            ConsumeFuel(consumption);
        }
        else
        {
            // При отсутствии ввода или топлива замедляем поезд до 0
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }

        // Перемещаем поезд по локальной оси X (transform.right)
        transform.position += transform.right * currentSpeed * Time.fixedDeltaTime;
    }

    // Метод для уменьшения количества топлива
    void ConsumeFuel(float amount)
    {
        currentFuel -= amount;
        if (currentFuel < 0)
            currentFuel = 0;
    }

    // Метод для добавления топлива
    public void AddFuel(float amount)
    {
        currentFuel += amount;
    }

    // Внешний метод для установки режима вождения
    public void SetPlayerOnSeat(bool onSeat)
    {
        playerOnSeat = onSeat;
        if (onSeat)
        {
            // Сбрасываем ввод и запускаем таймер игнорирования остаточного ввода
            ignoreInputTime = ignoreInputDuration;
            inputValue = 0;
        }
    }
}
