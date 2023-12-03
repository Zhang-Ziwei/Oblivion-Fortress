using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;

public class HurtUI : MonoBehaviour
{

    public void Init(float damage, Transform target, bool isCrit) {

        // instantiate the hurtUI prefab as a child of enemy canvas
        GameObject hurtUIInstance = Instantiate(gameObject);
        hurtUIInstance.transform.SetParent(target.Find("Canvas"));

        // set x 0.4, y 0.5
        hurtUIInstance.transform.localPosition = new Vector3(0.5f, 0.7f, 0f);

        // get tmp text component
        TextMeshProUGUI text = hurtUIInstance.GetComponent<TextMeshProUGUI>();
        text.text = "-" + damage.ToString();

        // if is crit, set color to red, else set color to white
        if (isCrit) {
            text.color = Color.red;
        } else {
            text.color = Color.white;
        }
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}
