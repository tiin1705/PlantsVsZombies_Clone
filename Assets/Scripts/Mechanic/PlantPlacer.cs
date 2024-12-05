using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPlacer : MonoBehaviour
{
    public PlantFactory PlantFactory;
    private GameObject plantGhosts;
    private bool isPlacing = false;

    public LayerMask gridLayer;
    private string selectedPlantType;

    private GridManager gridManager; // Tham chiếu đến GridManager

    public delegate void OnCanclePlacingDelegate();
    public OnCanclePlacingDelegate onCanclePlacing;
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();

    }
    void Update()
    {
        if (isPlacing)
        {
            // Tạo raycast để theo dõi vị trí chuột
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gridLayer);

            if (hit2D)
            {
                Vector3 gridPosition = GetGridPosition(hit2D.point);
                plantGhosts.transform.position = gridPosition;

                // Kiểm tra nếu người dùng nhấp chuột để đặt cây
                if (Input.GetMouseButtonDown(0))
                {
                    PlacePlant(gridPosition); // Đặt cây tại vị trí này
                }
            }
            else
            {
                // Nếu nhấp ra ngoài grid, ẩn bóng mờ và quay lại trạng thái chưa chọn
                if (Input.GetMouseButtonDown(0))
                {
                    CancelPlacing(); // Hủy quá trình đặt cây
                }
            }
        }
    }
    // Hàm tính toán vị trí lưới dựa trên điểm hit

    Vector3 GetGridPosition(Vector3 hitPoint)
    {

        // Tính toán chỉ số hàng và cột
        int colIndex = Mathf.RoundToInt((hitPoint.x - gridManager.gridOrigin.x) / gridManager.tileWidth);
        int rowIndex = Mathf.RoundToInt((hitPoint.y - gridManager.gridOrigin.y) / gridManager.tileHeigh);

        // Tính tọa độ chính giữa của ô dựa vào vị trí hàng và cột đã làm tròn
        float x = gridManager.gridOrigin.x + colIndex * gridManager.tileWidth;
        float y = gridManager.gridOrigin.y + rowIndex * gridManager.tileHeigh;


        // Debug.Log($"Calculated Grid Position: ({x}, {y})"); // Debug position

        return new Vector3(x, y, 0);
    }

    // Hàm để làm cho bóng mờ trong suốt hơn
    void SetGhostTransparency(GameObject ghost, float alpha)
    {
        SpriteRenderer[] spriteRenderers = ghost.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    //Hàm đặt plant tại vị trí đã chọn
    void PlacePlant(Vector3 position)
    {
        // Kiểm tra xem vị trí có trống hay không
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Plant")) // Nếu có cây ở vị trí đó
            {
                Debug.Log("Không thể đặt cây ở vị trí này! Đã có cây ở đây");
                return; // Không đặt cây
            }
        }

        // Lấy chi phí của loại cây
        int cost = PlantFactory.GetPlantCost(selectedPlantType);

        // Trừ tiền sau khi đặt cây
        if (SunManager.Instance.SpendSun(cost))
        {
            // Đặt cây thật tại vị trí lưới
            Plant plant = PlantFactory.CreatePlant(selectedPlantType);
            if (plant != null)
            {
                plant.transform.position = position;
                StartCooldownForPlant(selectedPlantType);
            }
            else
            {
                Debug.LogError("Plant creation failed!");
            }

            // Huỷ bóng mờ và kết thúc quá trình đặt
            Destroy(plantGhosts);
            isPlacing = false; // Kết thúc quá trình đặt
            if (onCanclePlacing != null)
            {
                onCanclePlacing?.Invoke();
            }
        }
        else
        {
            Debug.Log("Không đủ Sun để đặt cây!");
        }
    }

    //Hàm này được gọi khi nhấn nút UI để bắt đầu quá trình đặt plant
    public void StartingPlacingPlant(string plantType)
    {
        if (plantGhosts != null) return; // Nếu bóng mờ đã được tạo ra, không cho nhấn nút nữa

        selectedPlantType = plantType;
        isPlacing = true;

        // Tạo một bóng mờ từ prefab của cây thực sự
        GameObject ghostPrefab = PlantFactory.GetGhostPrefab(selectedPlantType);
        if (ghostPrefab != null)
        {
            plantGhosts = Instantiate(ghostPrefab);
            SetGhostTransparency(plantGhosts, 0.5f);
            plantGhosts.tag = "Ghost";
        }
    }


    void CancelPlacing()
    {
        Destroy(plantGhosts); // Xóa bóng mờ
        isPlacing = false; // Quay lại trạng thái chưa chọn cây
        if (onCanclePlacing != null)
        {
            onCanclePlacing?.Invoke();
        }
    }
    void StartCooldownForPlant(string plantType)
    {
        foreach(var buttonData in SunManager.Instance.plantButton)
        {
            if(buttonData.plantType == plantType)
            {
                buttonData.cooldownRemaining = PlantFactory.GetPlantCooldown(plantType);
                break;
            }
        }
    }

}