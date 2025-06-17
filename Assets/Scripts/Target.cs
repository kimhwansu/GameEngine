using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UJ.Attributes;
using UJ.DI;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;
    private GameManager gameManager;

    public int pointValue;
    public ParticleSystem explosionParticle;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
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
            gameManager.UpdateScore(pointValue);
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
