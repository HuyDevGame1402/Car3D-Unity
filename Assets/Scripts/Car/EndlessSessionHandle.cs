using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSessionHandle : MonoBehaviour
{
    // biến lưu player car
    [SerializeField] private Transform playerCarTransform;
    // Start is called before the first frame update
    void Start()
    {
        // tìm player car trong scene và đặt vào trong biến
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // update cho section nhg có thể sd hướng dot hay esc để giảm hàm update đi
    void Update()
    {
        // tính khoảng cách của section nó đến với player car 
        // kc là trục z
        float distanceToPlayer = transform.position.z - playerCarTransform.position.z;
        // tính độ lerp % xem cần lerp trục y lên tầm bn %
        // ta cần thông số là 100 chính là lúc player thấy hoàn chỉnh section y = 0 nhồi lên hoàn chỉnh
        // 150 là kc cần bắt đầu nhồi lên tức là ví dụ
        // đang ở 100 + 150 = 250 tức là lúc ý cb nhồi lên r và nhồi lên trong tg car đi 150 
        // tức trong lúc car đi đc 150 thì kc còn 250 - 150 = 100 
        // thì trong lúc đi đc 150 đó p nhồi lên đủ 100%
        // vậy cần tính lerp % dis - 100 xem còn bn chia cho 150 để bt đc % cần nhồi lên bn của y = -10

        float lerpPercentage = 1.0f - ((distanceToPlayer - 100) / 150.0f);
        lerpPercentage = Mathf.Clamp01(lerpPercentage); // giới hạn 0-1
        // sử dụng hàm lerp để tính toán trục y theo % 0 - 1
        // tính từ -10 - 0 với độ % 0 - 1
        // ví dụ 0.1 tức 10% di chuyển từ -10 -> 0 thì sẽ ra -9
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, -10, transform.position.z),
            new Vector3(transform.position.x, 0, transform.position.z), lerpPercentage);
    }
}
