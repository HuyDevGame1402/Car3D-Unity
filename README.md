# 🏎️ Car3D - Unity Racing Project

Dự án mô phỏng điều khiển xe 3D được xây dựng bằng Unity Engine, tập trung vào trải nghiệm lái xe chân thực và tối ưu hóa hiệu suất.

## 🚀 Tính năng chính (Key Features)

* **Vehicle Physics:** Hệ thống lái xe sử dụng `Wheel Colliders` kết hợp với tùy chỉnh trọng tâm (Center of Mass) để chống lật.
* **Smooth Camera:** Hệ thống camera bám đuổi (Follow Camera) với các hiệu ứng mượt mà bằng code hoặc Cinemachine.
* **Visual Effects:** Tích hợp hiệu ứng chuyển động, hiển thị Sprite và hiệu ứng UI sinh động.

## 🛠️ Công nghệ sử dụng (Tech Stack)

* **Game Engine:** Unity (Version 2022.3 LTS hoặc mới hơn).
* **Ngôn ngữ:** C# (Object-Oriented Programming).
* **Render Pipeline:** Universal Render Pipeline (URP) giúp tối ưu hóa đồ họa.
* **Version Control:** Git & GitHub (Quản lý dự án theo từng giai đoạn phát triển).

## 📁 Cấu trúc dự án (Folder Structure)

```text
Assets/
├── KayKit_City_Builder_Bits/ # Tài nguyên xây dựng thành phố (Môi trường)
├── KayKit_HalloweenBits/     # Tài nguyên chủ đề Halloween
├── Low Poly Weapons VOL.1/   # Mô hình vũ khí Low Poly
├── BrokenVector/             # Assets từ bên thứ ba (Môi trường/Xe)
├── Scenes/                   # Các màn chơi và môi trường mô phỏng
├── Scripts/                  # Logic điều khiển xe, hệ thống và xử lý gameplay
├── Prefabs/                  # Các đối tượng xe và công trình đã đóng gói sẵn
├── Materials/                # Vật liệu hiển thị cho các đối tượng 3D
├── Physics Material/         # Cấu hình vật lý (Ma sát, độ nảy của xe/đường)
├── Animation Local/          # Các diễn họa và chuyển động tùy chỉnh
├── SFX/                      # Hiệu ứng âm thanh (Tiếng động cơ, va chạm)
├── URP/                      # Cấu hình Universal Render Pipeline
└── Mixer/                    # Cấu hình âm thanh (Audio Mixer)
