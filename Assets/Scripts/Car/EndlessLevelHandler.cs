using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelHandler : MonoBehaviour
{
    // lưu các prefab của đoạn đường road 1 road 2 road 3
    [SerializeField] private GameObject[] sectionPrefabs;
    // array pool tạo sẵn 20 sestion pool khi cần thì bật lên không thì tắt đi
    // tránh instance liên tục và destroy liên tục rồi
    [SerializeField] private GameObject[] sectionsPool = new GameObject[20];
    // 10 section này sẽ hiển thị trc mặt ng chs
    [SerializeField] private GameObject[] sections = new GameObject[10];
    // xe của player xem transform.position.z của player đang ở đâu để tạo ra section
    [SerializeField] private Transform playerCarTransform;

    // biến để phục vụ coroutine chạy mỗi 0.1f để giảm lag
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    // chiều dài của 1 đoạn đường sestion tầm 26 units
    const float sectionLength = 26;

    // hàm start
    private void Start()
    {
        // tìm player trong scene và gán reference vào trong playerCarTransform
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        // tạo 20 prefab trong sectionsPool
        int prefabIndex = 0;
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            // tạo instantiate ra các prefab và đặt vào rong sectionPool
            sectionsPool[i] = Instantiate(sectionPrefabs[prefabIndex]);
            // tắt active hết đi
            sectionsPool[i].SetActive(false);
            // tăng chỉ số index
            prefabIndex++;
            // nếu lớn hơn length - 1 thì sẽ quay lại về 0 k thì k để tạo đc vì index quá k có
            if(prefabIndex > sectionPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        } 
        // tạo ra 10 cái active để đi và để trong sections
        for(int i = 0; i < sections.Length; i++)
        {
            // lấy ra randomSection từ array pool
            GameObject randomSection = GetRandomSectionFromPool();
            // setup position cho việc rải ra 10 cái đường
            // về position.x k thay đổi, y có là -10 dưới mặt đất và sẽ nhô lên 
            // cái thay đổi ngay đó là cái position.z
            // lấy chỉ số i nhân vs lengthg road section ra đc z tăng dần
            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x,
                -10, i * sectionLength);
            // bật active lên true
            randomSection.SetActive(true);
            // gán vào sections[i] = section random đó
            
            sections[i] = randomSection;
        }
        // đến đây đã có array sectionsPool và sections hiển thị
        // tiếp tục chạy coroutine 
        StartCoroutine(UpdateLessOftenCO());
    }
    // hàm coroutine
    private IEnumerator UpdateLessOftenCO()
    {
        // vòng lặp while chạy liên tục
        while(true)
        {
            // gọi hàm updatesection position
            UpdateSectionPosition();
            // đợi 0.1f 
            // xong lại lặp tiếp tránh gọi liên tục
            yield return waitForSeconds;
        }
    }
    // phân tích hàm update section position
    private void UpdateSectionPosition()
    {
        // duyệt qua các section road
        for(int i = 0;i < sections.Length;i++)
        {
            // khi nào section road ở sau player và khoảng cách là > 26 ví dụ road z ở 1 và player z ở 28
            // 1 - 28 = -27 < -26 length section thỏa mãn if
            if(sections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                // lấy ra position của section 
                Vector3 lastScetionPosition = sections[i].transform.position;
                sections[i].SetActive(false); // tắt active section road
                // lấy section ms từ pool 
                sections[i] = GetRandomSectionFromPool();
                // setup lại position
                // lấy vị trí hiện tại + max đường là ra vị trí ms 
                // ví dụ đơn giản
                // i=0 -> z = 0
                // i = 1->z = 26
                //i = 2->z = 52
                //i = 3->z = 78
                // -> qua z = 0 thì bh z ms của road tiếp p hiển thị
                // đó là 104 đúng k cái tiếp theo của cái cuối vừa r
                // 0 + 26 * 4 thì 26 * 4 chính là max đg hiển thị start ban đầu
                // tất cả những thg muốn vượt lên p đi qua max đường từ vị trí của nó
                sections[i].transform.position = new Vector3(lastScetionPosition.x, -10, lastScetionPosition.z 
                    + sectionLength * sections.Length);
                sections[i].SetActive(true); // bật active
            }
        }
    }
    // cùng phân tích hàm này có tác dụng lấy ra random section road trong array pool
    private GameObject GetRandomSectionFromPool()
    {
        // lấy chỉ số index trong array từ 0 - (length - 1)
        int randomIndex = Random.Range(0, sectionsPool.Length);
        // biến bool để kt xem tìm đc section ms cần trả về chưa
        bool isNewSectionFound = false;
        // while true chạy vô tận khi nào tìm đc thì dừng
        while(!isNewSectionFound)
        {
            // nếu nó k active bật lên thì oke tìm thấy vì cái đó tắt là có thể dùng đc
            if(!sectionsPool[randomIndex].activeInHierarchy)
            {
                // trả về true để thoát while
                isNewSectionFound = true;
            }
            else
            {
                // ngược lại nếu cái đó mà đang active
                randomIndex++; // tăng index lên 1 đơn vị
                if(randomIndex > sectionsPool.Length - 1)
                {
                    // vượt quá length thì trả về 0 ban đầu và tiếp tục while
                    randomIndex = 0;
                }
            }
        }
        // xong while tức là index đó thỏa mãn đk active đang flase
        // tức index đó là cái có thể dùng
        // trả về gameobject referance trong sectionsPool tại index đó
        return sectionsPool[randomIndex];
    }
}
