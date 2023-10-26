using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lớp thông báo khi có điểm
public class CongratulationWritings : MonoBehaviour
{
    // Danh sách thông báo có điểm
    public List<GameObject> writings;

    void Start()
    {
        GameEvents.ShowCongratulationWritings += ShowCongratulationWritings;
    }

    private void OnDisable()
    {
        GameEvents.ShowCongratulationWritings -= ShowCongratulationWritings;
    }

    // Hàm gọi thông báo có điểm
    private void ShowCongratulationWritings()
    {
        var index = UnityEngine.Random.Range(0, writings.Count);
        writings[index].SetActive(true);
    }
}
