using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UJ.DI;
using UJ.Data;
using UJ.Attributes;

[Injectable]
public class Target : MonoBehaviour
{
    [Inject] private GameManager gameManager;
    [Inject] private TargetData targetData;
    private Rigidbody targetRb;
    public ParticleSystem explosionParticle;

    void Start()
    {
        if (targetData == null)
        {
            Debug.LogError($"TargetData not injected for prefab: {gameObject.name}");
            return;
        }

        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

Vector3 RandomForce()
{
    return Vector3.up * Random.Range(targetData.minSpeed, targetData.maxSpeed);
}

float RandomTorque()
{
    return Random.Range(-targetData.maxTorque, targetData.maxTorque);
}

Vector3 RandomSpawnPos()
{
    return new Vector3(Random.Range(-targetData.xRange, targetData.xRange), targetData.ySpawnPos);
}

    private void OnMouseDown()  //마우스 클릭으로 씬뷰 오브젝트 삭제 + 점수 5점 증가
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not injected properly in Target");
            return;
        }

        if (explosionParticle == null)
        {
            Debug.LogError("ExplosionParticle is not set in Target");
            return;
        }

        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(targetData.pointValue);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); //파티클 현재 위치와 회전값 적용
        }
    }

    private void OnTriggerEnter(Collider other) //트리거가 있는 콜라이더와 충돌 시 하이라키에 있는 충돌한 오브젝트 삭제 (여기선 Sensor 오브젝트)
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not injected properly in Target");
            return;
        }

        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
    }
}
