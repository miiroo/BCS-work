using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Text logText;
    public GameObject playerView;
    public GameObject bullet;
    public Transform firePoint;
    public string prefabName;
    private bool swordAutoAttack = false;



    private void Start() {
        prefabName = "Prefabs/swordAttack1";
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerView.GetComponent<playerController>().animFinished && playerView.GetComponent<playerController>().attackAnimation) {
            Shoot(playerView.GetComponent<playerController>().skill, playerView.GetComponent<playerController>().weapon);
        }
    }


    void Shoot(int skillN, int weaponN) {
        if (weaponN == 0 ) { //sword
            swordAutoAttack = false;
            if (skillN == 0) swordAutoAttack = true;
            if (skillN == 1) prefabName = "Prefabs/swordAttack1"; //up
            if (skillN == 2) prefabName = "Prefabs/swordieSkill4"; //down
            if (skillN == 3) prefabName = "Prefabs/swordSkill2"; //left
            if (skillN == 4) prefabName = "Prefabs/swordSkill3"; //right
        }
        if (weaponN == 1) { // bow
            swordAutoAttack = false;
            if (skillN == 0) prefabName = "Prefabs/arrow";
            if (skillN == 1) prefabName = "Prefabs/arrowSkill1"; //up
            if (skillN == 2) prefabName = "Prefabs/sprt"; //down
            if (skillN == 3) prefabName = "Prefabs/archerSkill3"; //left
            if (skillN == 4) prefabName = "Prefabs/archerSkill4"; //right
        }
        if (weaponN == 2) { // wand
            swordAutoAttack = false;
            if (skillN == 0) prefabName = "Prefabs/fireBall";
            if (skillN == 1) prefabName = "Prefabs/wandSkill1"; //up
            if (skillN == 2) prefabName = "Prefabs/wandSkill2"; //down
            if (skillN == 3) prefabName = "Prefabs/wandSkill3"; //left
            if (skillN == 4) prefabName = "Prefabs/wandSkill4"; //right
        }
        if (!swordAutoAttack) {
            bullet = Resources.Load<GameObject>(prefabName);
            Vector3 newPosition = firePoint.position;
            newPosition.z = 0;
            if (playerView.transform.rotation.y == 0) newPosition.x += bullet.GetComponent<Bullet>().shiftX;
            else newPosition.x -= bullet.GetComponent<Bullet>().shiftX;
            newPosition.y -= bullet.GetComponent<Bullet>().shiftY;
            newPosition.z = 0;
            Instantiate(bullet, newPosition, playerView.transform.rotation);
            logText.text = "Prefab created " + prefabName; 
        }
        
    }
}
