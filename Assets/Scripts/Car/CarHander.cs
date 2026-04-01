using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarHander : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    // model của xe dùng để xoay visual theo hướng trượt
    [SerializeField] private Transform gameModel;

    [SerializeField] private ExplodeHandler explodeHandler;
    [SerializeField] private bool isExploded = false;

    [SerializeField] private MeshRenderer carMeshRender;
    [SerializeField] private int _EmissionColor = Shader.PropertyToID("_EmissionColor");
    [SerializeField] private Color emissiveColor = Color.white;
    [SerializeField] private float emissiveColorMultiplier = 0f;

    // lực tăng tốc
    [SerializeField] private float accelerationMultiplier = 3;
    // lực phanh
    [SerializeField] private float braksMultiplier = 15;
    // lực đánh lái
    [SerializeField] private float steeringMultiplier = 5;
    // input lái xe đầu vào
    // x là đánh lái trái phải
    // y là tiến lùi
    [SerializeField] private Vector2 input = Vector2.zero;
    // giới hạn tốc độ
    // tốc độ trượt ngang tối đa
    [SerializeField] private float maxSterrVelocity = 2f;
    // tốc độ tiến tối đa
    [SerializeField] private float maxForwardVelocity = 30;

    [SerializeField] private bool isPlayer = false;

    private void Start()
    {
        isPlayer = CompareTag("Player");
    }

    private void Update()
    {
        if (isExploded) return;
        // khi xe trượt sang ngang thì sẽ quay model sang ngang
        // kiểu giống oto hay xe máy quay bánh xe khi đánh lái sang ngang
        // thì xe sẽ quay nhẹ sang ngang
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);
        if(carMeshRender != null)
        {
            // độ sáng mong muốn của emission
            float desiredCarEmissiveColorMultiplier = 0f;
            // phanh thì đèn sáng mạnh hơn kiểu đèn phanh xe thường ý
            if (input.y < 0) desiredCarEmissiveColorMultiplier = 4.0f;
            // sd lerp để chuyển giá trị độ sáng hiện tại sang độ sáng mới mượt hơn
            emissiveColorMultiplier = Mathf.Lerp(emissiveColorMultiplier, desiredCarEmissiveColorMultiplier, Time.deltaTime * 4);
            // set color cho _EmissionColor của material của car xe để thay đổi độ sáng màu sắc
            carMeshRender.material.SetColor(_EmissionColor, emissiveColor * emissiveColorMultiplier);
        }
    }

    private void FixedUpdate()
    {
        if (isExploded)
        {
            rb.drag = rb.velocity.z * 0.1f;
            rb.drag = Mathf.Clamp(rb.drag, 1.5f, 10);
            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0,0, transform.position.z),
                Time.deltaTime * 0.5f));
            return;
        }
        if(input.y > 0)
        {
            // nếu ấn w tức input.y > 0 thì dĩ nhiên r có tốc độ 
            // đầu vào phải đi lên thì gọi hàm tiến
            Accelerate();
        }
        else
        {
            // ngược lại nếu đầu vào k có tức nhả tay ra
            // thì cần tăng drag để xe tự chậm lại thoi
            rb.drag = 0.2f;
        }
        if(input.y < 0)
        {
            // nếu y âm tức là phanh
            Brake();
        }
        // hàm đánh lái sang trái phải
        Steer();
        // nếu xe bị đẩy ngc lại tức lực z <= 0 thì 
        // chỉnh vận tốc = 0 
        // k cho xe lùi lại
        if(rb.velocity.z <= 0)
        {
            rb.velocity = Vector3.zero;
        }
    }
    // hàm tiến tăng tốc
    private void Accelerate()
    { 
        // khi tăng tốc dĩ nhiên p bỏ đi drag
        // drag giống nhanh hay lực cản ý bỏ đi
        rb.drag = 0;
        // nếu tốc độ lớn quá lớn hơn max thì return
        if (rb.velocity.z >= maxForwardVelocity) return;
        // dùng lực của chính xe
        // hướng forward của xe * lực xe * input đầu vào y
        // hàm này sẽ tính ra vector 3 theo hướng + lực + input
        // sau đó lấy ra z do forward luôn là (0, 0, 1);
        // do đó z có lực
        // từ đó tính ra + vs velocity -> tăng position
        // mỗi vòng lặp đề cộng vào velocity xong ms có đk ở trên
        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);
    }

    // hàm phanh lại
    private void Brake()
    {
        // nếu xe đang đứng hoặc lùi thì return;
        if (rb.velocity.z <= 0) return;
        // phanh xe lại lúc này thì trừ velocity do là input.y âm
        // thì velocity sẽ trừ dần -> xe chậm dần
        rb.AddForce(rb.transform.forward * braksMultiplier * input.y);
    }
    // hàm đánh lái trái phải
    private void Steer()
    {
        if(Mathf.Abs(input.x) > 0)
        {
            // nếu có input trái âm phải dương nên p sd abs trị tuyệt đối
            // giới hạn đánh lái như bạn thấy thì velocity.z càng lớn thì limit càng lớn
            // nếu = 0 thì tức velocity k tiến thì k thể đánh lái
            float speedBaseTreerLimit = rb.velocity.z / 5.0f;
            speedBaseTreerLimit = Mathf.Clamp01(speedBaseTreerLimit);
            // thực hiện đánh lái right x * lực đánh * đầu vào input * với lực limit giới hạn
            rb.AddForce(rb.transform.right * steeringMultiplier * input.x * speedBaseTreerLimit);
            // chuẩn hóa hướng x
            float normalizedX = rb.velocity.x / maxSterrVelocity;
            // chuẩn hóa về trong khoảng -1 -> 1
            // ví dụ đang 0.5 thì oke vẫn trong giữ nguyên
            // 1.3 -> 1; -1.3 -> -1
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);  
            // xong * lại vs max = 2
            // mà trên giới hạn -1 -> 1 r nên bh là -2 -> 2
            rb.velocity = new Vector3(normalizedX * maxSterrVelocity, 0, rb.velocity.z);
        }
        else
        {
            // giảm dần về 0 khi k đánh lái xe nữa
            // khi k đánh lái xe vẫn trượt về hướng cũ do lực ma sát
            // lực bám đường ,....
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
            
        }
    }

    private IEnumerator SlowDownTimeCO()
    {
        while(Time.timeScale > 0.2f)
        {
            Time.timeScale -= Time.deltaTime * 2;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 1.0f;
    }

    // hàm set input hàm bình thg 
    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
    public void SetMaxSpeed(float newMaxSpeed)
    {
        maxForwardVelocity = newMaxSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isPlayer)
        {
            if(collision.transform.root.CompareTag("Untagged")) return;

            if (collision.transform.root.CompareTag("CarAI")) return;
        }

        Vector3 velocity = rb.velocity;
        explodeHandler.Explode(velocity * 45);
        isExploded = true;
        StartCoroutine(SlowDownTimeCO());
    }
}
