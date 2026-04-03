using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{
    // Object gốc xe nguyên vẹn
    [SerializeField] private GameObject orignalObject;
    // model là gameobject cha chứa các mảnh model vỡ của xe
    [SerializeField] private GameObject model;
    // danh sách các mảnh có rigibody để tác dụng lực
    [SerializeField] private Rigidbody[] rigidbodies;

    private void Awake()
    {
        // lấy tất cả các mảnh rigibody trong model vỡ của xe làm con của model
        rigidbodies = model.GetComponentsInChildren<Rigidbody>(true);
    }
    private void Start()
    {
        //Explode(Vector3.forward);
    }
    public void Explode(Vector3 externalForce)
    {
        // set orignalObject gốc của xe nguyên vẹn thành false để tắt đi
        orignalObject.SetActive(false);
        // duyệt qua tất cả các rigibody mảnh xe trong model
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            // k set parent tránh bị parent làm ảnh hưởng
            rigidbody.transform.parent = null;
            // bật meshcollider để có thể va chạm
            rigidbody.GetComponent<MeshCollider>().enabled = true;
            // bật active lên nếu có chỉnh false
            rigidbody.gameObject.SetActive(true);
            // bật lên để có thể bị ảnh hưởng bởi vật lý như physics hay gravity,...
            rigidbody.isKinematic = false;
            // bật interpolation để mảnh vỡ di chuyển mượt hơn khi bị tác dụng lực
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            // tác dụng lực lên mảnh vỡ để tạo hiệu ứng nổ
            // externalForce là lực bên ngoài có thể truyền vào để tạo hiệu ứng nổ mạnh hơn hoặc yếu hơn tùy ý
            // ForceMode.Force để tác dụng lực liên tục theo thời gian, ForceMode.Impulse để tác dụng lực ngay lập tức
            rigidbody.AddForce(Vector3.up * 200 + externalForce, ForceMode.Force);
            // tác dụng lực xoay ngẫu nhiên để tạo hiệu ứng mảnh vỡ quay lộn xộn khi nổ
            rigidbody.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);
            // đặt tag cho mảnh vỡ để có thể nhận biết là CarPart khi va chạm với các đối tượng khác như đường hay cây cối để tạo hiệu ứng va chạm
            rigidbody.gameObject.tag = "CarPart";
        }
    }

}
