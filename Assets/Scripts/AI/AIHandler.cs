using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    // carHander để điều khiển xe
    [SerializeField] private CarHander carHander;
    // otherCarsLayerMask để xác định layer của các xe khác để tránh va chạm
    [SerializeField] private LayerMask otherCarsLayerMask;
    // meshCollider để tắt đi khi raycast để tránh bị va chạm với chính nó
    [SerializeField] private MeshCollider meshCollider;
    // raycastHits để lưu kết quả của raycast khi kiểm tra có xe khác ở phía trước hay không
    [SerializeField] private RaycastHit[] raycastHits = new RaycastHit[1];
    // state xem có xe phía trước k
    [SerializeField] private bool isCarAhead = false;
    
    WaitForSeconds wait = new WaitForSeconds(0.2f);
    // index làn xe đang chạy
    int drivingInLane = 0;

    private void Awake()
    {
        // nếu là Player thì k dùng AI
        if (CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }
    private void Start()
    {
        // sử dụng coroutine để loop check xe phía trc 
        // k sử dụng update giảm cost
        StartCoroutine(UpdateLessOftenCO());
    }
    private void Update()
    {
        // input ga phanh cho xe
        // nếu là 1 là tiền 0 k ga , -1 là phanh
        float accelerationInput = 1.0f;
        // steer input để đánh lái cho xe
        float steerInput = 0.0f;
        // nếu có xe phía trước chuyển từ ga sang phanh để tránh va chạm
        if (isCarAhead) accelerationInput = -1f;

        // ví dụ drivingInLane = 0
        // thí desiredPositionX sẽ là Utils.CarLanes[0] = -0.3f 
        float desiredPositionX = Utils.CarLanes[drivingInLane];
        // tính toán sự khác biệt giữa vị trí hiện tại của xe và vị trí mong muốn của làn đường để điều chỉnh lái xe
        float difference = desiredPositionX - transform.localPosition.x;
        if(Mathf.Abs(difference) > 0.05f)
        {
            // ví dụ position X là -1
            // thì difference sẽ là -0.3 - (-1) = 0.7f
            // thì tức là nó cần đánh sang phải 1 lực 0.7 để về làn
            steerInput = 1.0f * difference;
        }
        // giới hạn chỉ ở trong -1 đến 1 để tránh đánh lái quá mạnh
        steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);
        // setinput hướng đi và hướng đánh lái cho carHander để điều khiển xe (CarAI)
        carHander.SetInput(new Vector2(steerInput, accelerationInput));
    }
    private IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            // cách 1 khoảng tg check 1 lần
            isCarAhead = CheckIfOtherCarsIsAhead();
            yield return wait;
        }
    }
    // kiểm tra có xe phía trc không
    // true là có, false là k có
    private bool CheckIfOtherCarsIsAhead()
    {
        // tắt meshCollider đi để tránh bị va chạm với chính nó khi raycast
        meshCollider.enabled = false;
        // sử dụng hàm BoxCastNonAlloc để kiểm tra có xe khác ở phía trước hay không
        // BoxCast giống raycast nhg thay vì tia thì nó là hình hộp box
        // transform.position là vị trí xuất phát của boxcast
        // Vector3.one * 0.25f là kích thước của boxcast, tức là 0.25f ở cả 3 trục x y z
        // transform.forward là hướng của boxcast, tức là hướng mà xe đang đi về phía trước
        // raycastHits là mảng để lưu kết quả của boxcast, tức là các va chạm mà boxcast gặp phải
        // Quaternion.identity là không có sự xoay nào cho boxcast, tức là nó sẽ thẳng đứng theo hướng forward
        // 2 là khoảng cách mà boxcast sẽ kiểm tra, tức là nó sẽ kiểm tra trong phạm vi 2 units phía trước xe
        // otherCarsLayerMask là layer mask để chỉ kiểm tra va chạm với các đối tượng thuộc layer của các xe khác, tức là nó sẽ bỏ qua các đối tượng không phải là xe khác
        // hàm này sẽ trả về số lượng va chạm mà boxcast gặp phải, nếu lớn hơn 0 thì có nghĩa là có xe khác ở phía trước
        int numberOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f,
            transform.forward, raycastHits, Quaternion.identity, 2, otherCarsLayerMask);
        meshCollider.enabled = true;
        // nếu có va chạm thì trả về true, ngược lại trả về false
        if(numberOfHits > 0) return true;

        return false;
    }
    private void OnEnable()
    {
        // set random max speed
        // và set random làn cho mỗi xe AI
        carHander.SetMaxSpeed(Random.Range(2, 4));
        drivingInLane = Random.Range(0, Utils.CarLanes.Length);
    }
}
